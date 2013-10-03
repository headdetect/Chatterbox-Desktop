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
using agsXMPP.protocol.iq.browse;
using agsXMPP.protocol.iq.disco;
using agsXMPP.protocol.x.muc;
using agsXMPP.Xml.Dom;
using Chatterbox.Plugins;

namespace Chatterbox.Hipchat
{
    class HipchatChatPlugin : ChatPlugin
    {
        internal const string LoginFormat = "version=1.20130116182826&email={0}&tls=1&password={1}";
        internal static List<string> Users = new List<string>();
        internal static XmlElement LoginData;

        internal static XmppClientConnection HipchatClient;
        internal static Jid SelfJid;
        internal static MucManager MucManager;

        internal static LobbyControl Lobby;

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

        public override void OnLoad(Gui.MainWindow window)
        {
            Window = window;

            var loginWindow = new LoginWindow();
            if (!loginWindow.ShowDialog().HasValue || LoginData == null)
            {
                window.Close();
                return;
            }

            SelfJid = new Jid(Nickname + "@chat.hipchat.com");

            window.Title = "Chatterbox - " + GroupName;

            HipchatClient = new XmppClientConnection("chat.hipchat.com");
            HipchatClient.Open(Nickname, loginWindow.Password);
            HipchatClient.Status = "DID IT WORK :D?";
            HipchatClient.Show = ShowType.chat;
            HipchatClient.AutoResolveConnectServer = false;
            HipchatClient.OnLogin += HipchatClient_OnLogin;

            MucManager = new MucManager(HipchatClient);

            HipchatClient.OnPresence += HipchatClient_OnPresence;
            HipchatClient.OnRosterItem += HipchatClient_OnRosterItem;
            HipchatClient.OnIq += HipchatClient_OnIq;
            HipchatClient.OnBinded += HipchatClient_OnBinded;
            HipchatClient.OnReadXml += HipchatClient_OnReadXml;
            Window.SetLobbyRoom(Lobby = new LobbyControl());
        }

        void HipchatClient_OnReadXml(object sender, string xml)
        {
            Lobby.Dispatcher.Invoke(new Action(() => Lobby.lstRooms.Items.Add("XML: " + xml)));
        }

        void HipchatClient_OnBinded(object sender)
        {
            //Lobby.Dispatcher.Invoke(new Action(() => Lobby.lstRooms.Items.Add("BIND: " + sender)));
        }

        void HipchatClient_OnIq(object sender, IQ iq)
        {
            Lobby.Dispatcher.Invoke(new Action(() => Lobby.lstRooms.Items.Add("IQ: " + iq)));
        }

        void HipchatClient_OnRosterItem(object sender, agsXMPP.protocol.iq.roster.RosterItem item)
        {
            Lobby.Dispatcher.Invoke(new Action(() => Lobby.lstRooms.Items.Add("ROST " + item.Name)));
        }

        void HipchatClient_OnLogin(object sender)
        {
            IQ iq = new IQ(IqType.get, SelfJid, new Jid("conf.hipchat.com")) { Query = new DiscoItems() };
            HipchatClient.Send(iq);

            //MucManager.JoinRoom(new Jid("room@conf.hipchat.com"), Name);
            //HipchatClient.Send(new Message(new Jid("room@conf.hipchat.com"), MessageType.groupchat, "This is a test message. Pls ignore k?"));


        }

        void HipchatClient_OnPresence(object sender, Presence pres)
        {
            Lobby.Dispatcher.Invoke(new Action(() => Lobby.lstRooms.Items.Add("PRES " + pres)));
        }


        public override bool OnJoinRoom(string room)
        {
            //TODO: Open SelectRoomWindow.xaml
            return true;
        }

        public override Gui.MessageBlock OnMessage(string room, string message)
        {
            //TODO: Parse and such.
            return string.Empty;
        }

        public static void DoLogin(string username, string password)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var responseString = client.UploadString("https://api-static-1.hipchat.com/api/connect_info", "POST", string.Format(LoginFormat, username, password));
                if (responseString.Contains("Invalid email or password.")) return;

                var loginDataDocument = new XmlDocument();
                loginDataDocument.LoadXml(responseString);
                LoginData = loginDataDocument["response"];
            }
        }
    }
}
