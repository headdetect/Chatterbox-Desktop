using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chatterbox.Gui;
using Chatterbox.Gui.Plugins;
using Chatterbox.Irc;
using Meebey.SmartIrc4net;

namespace Chatterbox
{
    public class IrcChatHandler : ChatPlugin
    {
        private IrcClient _client;
        private MainWindow _window;

        public override void OnLoad(MainWindow window)
        {
            _window = window;

            _client = IrcController.Client;
            _client.OnConnecting += client_OnConnecting;
            _client.OnConnected += client_OnConnected;
            _client.OnConnectionError += client_OnConnectionError;
            _client.OnChannelMessage += client_OnChannelMessage;
            _client.OnError += client_OnError;
            _client.OnErrorMessage += client_OnErrorMessage;
            _client.OnChannelNotice += client_OnChannelNotice;
            _client.OnChannelAction += client_OnChannelAction;
            _client.OnNames += client_OnNames;

            _window.CreateRoom("#bots");

            IrcController.Connect();
        }

        public override bool OnJoinRoom(string room)
        {
            return false;
        }

        public override MessageBlock OnMessage(string room, string message)
        {
            const bool canSend = true;

            //TODO: Make sure can send;
            _client.SendMessage(SendType.Message, room, message);

            return message;
        }

        void client_OnNames(object sender, NamesEventArgs e)
        {
            _window.AddUsers(e.UserList, e.Channel);
        }


        void client_OnConnecting(object sender, EventArgs e)
        {
            //AddItem( "Connecting..." );
        }

        void client_OnChannelNotice(object sender, IrcEventArgs e)
        {
            //TODO: Regex
            _window.AddItem(e.Data.Message.Replace('\x03', '\x200B').Replace('\x0F', '\x200B').Replace('\x02', '\x200B').Replace('\x1F', '\x200B'), e.Data.Nick, e.Data.Channel);
        }


        void client_OnChannelAction(object sender, ActionEventArgs e)
        {
            _window.AddItem("OnChannelAction-> " + e.ActionMessage, e.Data.Nick, e.Data.Channel);
        }



        void client_OnConnected(object sender, EventArgs e)
        {
            IrcController.Connected = true;
            //AddItem( "Connected!" );
        }



        void client_OnConnectionError(object sender, EventArgs e)
        {
            IrcController.Connected = false;
            //AddItem( "Error connecting" );
        }

        void client_OnChannelMessage(object sender, IrcEventArgs e)
        {
            //TODO: Remove color code removal

            _window.AddItem(e.Data.Message.Replace('\x03', '\x200B'), e.Data.Nick, e.Data.Channel);
        }

        void client_OnErrorMessage(object sender, IrcEventArgs e)
        {
            _window.AddItem("ERROR" + e.Data.Message, e.Data.Nick, e.Data.Channel);
        }

        void client_OnError(object sender, ErrorEventArgs e)
        {
            _window.AddItem("ERROR" + e.Data.Message, e.Data.Nick, e.Data.Channel);
        }
    }
}
