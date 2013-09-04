using System;
using System.Linq;
using Chatterbox.Gui;
using Chatterbox.Gui.Plugins;
using SKYPE4COMLib;
using System.Collections.Generic;

namespace Chatterbox.Skype
{
    public class SkypeChatHandler : ChatPlugin
    {
        internal SKYPE4COMLib.Skype _skype;
        private WindowHolder _wHolder;
        public override void OnLoad(MainWindow window)
        {
            _wHolder = new WindowHolder(window);
            _skype = new SKYPE4COMLib.Skype();
            if (!_skype.Client.IsRunning)
                _skype.Client.Start(true);
            _skype.Attach(8, false);
            _skype.MessageStatus += SkypeOnMessageStatus;
        }

        private void SkypeOnMessageStatus(ChatMessage pMessage, TChatMessageStatus status)
        {
            if (status == TChatMessageStatus.cmsReceived)
            {
                if (!_wHolder.DoesRoomExist(pMessage.Chat.FriendlyName))
                {
                    _wHolder.CreateRoom(pMessage.ChatName, pMessage.Chat);
                }
                //_wHolder.GetMainWindow()
                //    .AddItem("STATUS: " + status, "SYSTEM", pMessage.Chat.FriendlyName.Split('|')[0]);
                _wHolder.GetMainWindow()
                    .AddItem(pMessage.Body, pMessage.FromDisplayName, pMessage.Chat.FriendlyName.Split('|')[0]);
                pMessage.Seen = true;
            }
        }

        public override bool OnJoinRoom(string room)
        {
            //var ContactWindow = new SelectContactWindow(this);
            //ContactWindow.Show();
            //TODO Not implemented yet
            return false;
        }

        public override MessageBlock OnMessage(string room, string message)
        {
            if (!_wHolder.DoesRoomExist(room))
                return null;

            Chat chat = _wHolder.GetChatForRoom(room);
            chat.SendMessage(message);
            return message;
        }
    }

    class WindowHolder
    {
        private readonly Dictionary<string, Chat> _rooms = new Dictionary<string, Chat>();
        private readonly MainWindow _window;

        public WindowHolder(MainWindow window)
        {
            _window = window;
        }

        public void CreateRoom(string name, Chat chat)
        {
            if (DoesRoomExist(name))
                return;
            string room_name = chat.FriendlyName.Split('|')[0];
            _window.CreateRoom(room_name);
            string[] usernames = new string[chat.Members.Count];
            int i = 0;
            foreach (User u in chat.Members)
            {
                usernames[i] = u.FullName;
                i++;
            }
            _window.AddUsers(usernames, room_name);
            _rooms.Add(name, chat);
            
        }

        public bool DoesRoomExist(string name)
        {
            return _rooms.Any(s => name == s.Value.FriendlyName.Split('|')[0]);
        }

        public Chat GetChatForRoom(string name)
        {
            return _rooms.Where(s => s.Value.FriendlyName.Split('|')[0] == name).ToArray()[0].Value;
        }

        public MainWindow GetMainWindow()
        {
            return _window;
        }
    }
}
