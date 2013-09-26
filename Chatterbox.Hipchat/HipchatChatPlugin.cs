using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Chatterbox.Plugins;
using Matrix.Xmpp;
using Matrix.Xmpp.Client;

namespace Chatterbox.Hipchat
{
    class HipchatChatPlugin : ChatPlugin
    {
        internal const string LoginFormat = "version=1.20130116182826&email={0}&tls=1&password={1}";
        internal static List<string> Users = new List<string>();
        internal static XmlElement LoginData;

        internal static XmppClient HipchatClient;

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

        public override void OnLoad(Gui.MainWindow window)
        {
            var loginWindow = new LoginWindow();
            if (!loginWindow.ShowDialog().HasValue || LoginData == null)
            {
                window.Close();
                return;
            }


            window.Title = "Chatterbox - " + GroupName;

            HipchatClient = new XmppClient(Nickname, "conf.hipchat.com",  loginWindow.Password);
            HipchatClient.Open();
            HipchatClient.Status = "DID IT WORK :D?";
            HipchatClient.Show = Show.chat;

            HipchatClient.OnPresence += HipchatClient_OnPresence;

            window.SetLobbyRoom(new LobbyControl());


            //TODO: Load all rooms and users
        }

        void HipchatClient_OnPresence(object sender, PresenceEventArgs e)
        {
            // Users and such //
            throw new NotImplementedException();
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
                var responseString = client.UploadString("https://api-static-2.hipchat.com/api/connect_info", "POST", string.Format(LoginFormat, username, password));
                if (responseString.Contains("Invalid email or password.")) return;

                var loginDataDocument = new XmlDocument();
                loginDataDocument.LoadXml(responseString);
                LoginData = loginDataDocument["response"];
            }
        }
    }
}
