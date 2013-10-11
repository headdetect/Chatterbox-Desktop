using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.component;
using agsXMPP.protocol.iq.browse;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.iq.roster;
using agsXMPP.protocol.x.muc;
using agsXMPP.Xml.Dom;
using Chatterbox.Hipchat.Model;
using Chatterbox.Plugins;
using Message = agsXMPP.protocol.client.Message;

namespace Chatterbox.Hipchat
{
    class HipchatChatPlugin : ChatPlugin
    {
        internal const string LoginFormat = "version=1.20130116182826&email={0}&tls=1&password={1}";
        internal static List<RosterItem> Users = new List<RosterItem>();
        internal static XmlElement LoginData;

        internal static XmppClientConnection HipchatClient;
        internal static Jid SelfJid;
        internal static MucManager MucManager;

        internal static LobbyControl Lobby;

        internal static List<HipchatRoom> Rooms = new List<HipchatRoom>();

        #region Hipchat Vars

        internal static string Nickname
        {
            get
            {
                var ret = LoginData["nickname"];
                return ret != null ? ret.InnerText : null;
            }
        }

        internal static string Mention
        {
            get
            {
                var ret = LoginData["mention"];
                return ret != null ? ret.InnerText : null;
            }
        }

        internal static string Name
        {
            get
            {
                var ret = LoginData["name"];
                return ret != null ? ret.InnerText : null;
            }
        }

        internal static string UserID
        {
            get
            {
                var ret = LoginData["user_id"];
                return ret != null ? ret.InnerText : null;
            }
        }

        internal static string GroupID
        {
            get
            {
                var ret = LoginData["group_id"];
                return ret != null ? ret.InnerText : null;
            }
        }

        internal static string GroupName
        {
            get
            {
                var ret = LoginData["group_name"];
                return ret != null ? ret.InnerText : null;
            }
        }

        internal static bool IsAdmin
        {
            get
            {
                var ret = LoginData["nickname"];
                return ret != null && ret.InnerText == "1";
            }
        }

        #endregion

        internal static Gui.MainWindow Window;

        internal static Configuration Config;

        public override bool OnLoad(Gui.MainWindow window)
        {
            Config = Configuration.Load();

            Window = window;

            var loginWindow = new LoginWindow { txtUsername = { Text = Config.Email } };
            if (!loginWindow.ShowDialog().HasValue || LoginData == null)
            {
                return false;
            }

            Config.Email = loginWindow.Username;

            if (loginWindow.chkRemember.IsChecked != null && loginWindow.chkRemember.IsChecked.Value)
            {
                Config.Save();
            }

            SelfJid = new Jid(Nickname + "@chat.hipchat.com");

            window.Title = "Chatterbox - " + GroupName;

            HipchatClient = new XmppClientConnection("chat.hipchat.com");
            HipchatClient.Open(Nickname, loginWindow.Password);
            HipchatClient.Status = "Chatterbox";
            HipchatClient.Show = ShowType.chat;
            HipchatClient.AutoResolveConnectServer = false;
            HipchatClient.OnLogin += HipchatClient_OnLogin;

            MucManager = new MucManager(HipchatClient);

            HipchatClient.OnRosterStart += sender => Users.Clear();
            HipchatClient.OnRosterItem += HipchatClient_OnRosterItem;
            HipchatClient.OnMessage += HipchatClient_OnMessage;
            HipchatClient.OnPresence += HipchatClient_OnPresence;

            Lobby = new LobbyControl();
            Lobby.OnRoomJoin += Lobby_OnRoomJoin;


            Window.SetLobbyRoom(Lobby);

            return true;
        }

        static void HipchatClient_OnPresence(object sender, agsXMPP.protocol.client.Presence pres)
        {
            HipchatRoom room = Rooms.FirstOrDefault(rm => rm.RoomId.Bare.StartsWith(pres.From.Bare, StringComparison.CurrentCultureIgnoreCase));
            if (room == null) return;

            if(!room.Users.Contains(pres.From.Resource))
                room.Users.Add(pres.From.Resource);

            Window.SetUsers(room.Users.ToArray(), room.Name);

            // Lobby.Dispatcher.Invoke(new Action(() => Lobby.lstRooms.Items.Add(pres.ToString())));
        }


        static void HipchatClient_OnMessage(object sender, Message msg)
        {
            HipchatRoom room = Rooms.FirstOrDefault(rm => rm.RoomId.Bare.StartsWith(msg.From.Bare, StringComparison.CurrentCultureIgnoreCase));
            if (room == null) return;
            Window.AddItem(msg.Body, msg.From.Resource, room.Name);
        }

        static void Lobby_OnRoomJoin(object sender, LobbyControl.RoomJoinEventArgs e)
        {           
            MucManager.JoinRoom(new Jid(e.HipchatRoom.RoomId), Name);

            Window.CreateRoom(e.HipchatRoom.Name);
        }


        static void HipchatClient_OnRosterItem(object sender, RosterItem item)
        {
            Users.Add(item); 
            Lobby.DoUserCheck();
        }

        static void HipchatClient_OnLogin(object sender)
        {
            HipchatClient.OnIq += (e, mIq) =>
            {
                if (mIq.Type == IqType.error) return;
                DiscoItems item = mIq.Query as DiscoItems;
                if (item == null) return;
                var items = item.GetDiscoItems();
                foreach (HipchatRoom hipChatRoom in items.Select(room => new HipchatRoom(room.Name, room.Jid)))
                {
                    Rooms.Add(hipChatRoom);
                    Lobby.DoRoomCheck();
                }
            };
            DiscoManager mgnr = new DiscoManager(HipchatClient);
            mgnr.DiscoverItems(new Jid("conf.hipchat.com"));
        }

        public override Gui.MessageBlock OnMessage(string room, string message)
        {
            HipchatRoom hRoom = Rooms.FirstOrDefault(r => r.Name.Equals(room));
            if (hRoom == null) return message;
            HipchatClient.Send(new Message(new Jid(hRoom.RoomId), MessageType.groupchat, message));
            return message;
        }

        private static bool _reconnectedOnSslFail;
        public static void DoLogin(string username, string password)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    var responseString = client.UploadString("https://api-static-1.hipchat.com/api/connect_info", "POST", string.Format(LoginFormat, username, password));
                    if (!responseString.Contains("<response><name>")) return; // The first two tags in a successful login. //

                    var loginDataDocument = new XmlDocument();
                    loginDataDocument.LoadXml(responseString);
                    LoginData = loginDataDocument["response"];
                }
            }
            catch(WebException e)
            {
                if (e.Message.Contains("SSL") && !_reconnectedOnSslFail)
                {
                    _reconnectedOnSslFail = true;

                    // Sometimes the ssl certificate can't be verified until you try it again... //
                    DoLogin(username, password);
                }
            }
        }
    }
}
