using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Meebey.SmartIrc4net;

namespace Chatterbox.Gui {
    public partial class MainWindow {

        void client_OnNames ( object sender, NamesEventArgs e ) {
            if ( !Reinvoke( client_OnNames, sender, e ) )
                return;

            AddUsers( e.UserList, e.Channel );
        }


        void client_OnConnecting ( object sender, EventArgs e ) {
            if ( !Reinvoke( client_OnConnecting, sender, e ) )
                return;

            //AddItem( "Connecting..." );
        }

        void client_OnChannelNotice ( object sender, IrcEventArgs e ) {
            if ( !Reinvoke( client_OnChannelNotice, sender, e ) )
                return;


            //TODO: Regex
            AddItem( e.Data.Message.Replace( '\x03', '\x200B' ).Replace( '\x0F', '\x200B' ).Replace( '\x02', '\x200B' ).Replace( '\x1F', '\x200B' ), e.Data.Nick, e.Data.Channel );
        }


        void client_OnChannelAction ( object sender, ActionEventArgs e ) {
            if ( !Reinvoke( client_OnChannelAction, sender, e ) )
                return;

            AddItem( "OnChannelAction-> " + e.ActionMessage, e.Data.Nick, e.Data.Channel );
        }



        void client_OnConnected ( object sender, EventArgs e ) {
            if ( !Reinvoke( client_OnConnected, sender, e ) )
                return;

            IrcController.Connected = true;
            //AddItem( "Connected!" );
        }



        void client_OnConnectionError ( object sender, EventArgs e ) {
            if ( !Reinvoke( client_OnConnectionError, sender, e ) )
                return;

            IrcController.Connected = false;
            //AddItem( "Error connecting" );
        }

        void client_OnChannelMessage ( object sender, IrcEventArgs e ) {
            if ( !Reinvoke( client_OnChannelMessage, sender, e ) )
                return;

            //TODO: Remove color code removal

            AddItem( e.Data.Message.Replace( '\x03', '\x200B' ), e.Data.Nick, e.Data.Channel );
        }

        void client_OnErrorMessage ( object sender, IrcEventArgs e ) {
            if ( !Reinvoke( client_OnErrorMessage, sender, e ) )
                return;

            AddItem( "ERROR" + e.Data.Message, e.Data.Nick, e.Data.Channel );
        }

        void client_OnError ( object sender, ErrorEventArgs e ) {
            if ( !Reinvoke( client_OnError, sender, e ) )
                return;

            AddItem( "ERROR" + e.Data.Message, e.Data.Nick, e.Data.Channel );
        }
    }
}
