using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chatterbox.Gui.Converters;
using Meebey.SmartIrc4net;

namespace Chatterbox.Gui.Controls {

    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow {

        public string Channel { get; set; }

        public ChatWindow () {
            InitializeComponent();
#if DESIGN
            AddItem( "This is a line message that would be inserted here of sorts", "", true );
            AddItem( "This is a line message that would be inserted here of sorts", "headdetect", true );


            AddItem( "System message goes here", "", true );

            AddItem( "http://www.youtube.com/watch?v=0tCi6kj56Dg", "headdetect", true );
#elif DEBUG
            Channel = "#bots";
#endif
        }


        private void btnSend_Click ( object sender, RoutedEventArgs e ) {

#if !DESIGN
            if ( !IrcController.Connected || IrcController.Client == null )
                return;
#endif

            string text = txtMessage.Text;
#if DESIGN
            AddItem( text, "headdetect" );
#else
            IrcController.Client.SendMessage( SendType.Message, Channel, text );
#endif


            txtMessage.Clear();
        }

        private void txtMessage_KeyDown ( object sender, KeyEventArgs e ) {
            if ( e.Key == Key.Enter ) {
                btnSend_Click( null, e );
            }
        }

        public void AddItem ( string message, string user = "", bool isSystem = false ) {
            if ( lstChat == null || string.IsNullOrWhiteSpace( message ) )
                return;

            if ( !isSystem ) {
                message = message.Replace( StringToXamlConverter.ParseKey, "\x200B§\x200B" );
            }

            if ( string.IsNullOrWhiteSpace( user ) ) {
                message = StringToXamlConverter.ParseKey + "~#666~" + StringToXamlConverter.ParseKey + message + StringToXamlConverter.ParseKey + "~E~" +
                          StringToXamlConverter.ParseKey;
            }

            lstChat.Items.Add( new ChatItem( user, message ) );
        }

        public void AddUser ( string[] users ) {
            for ( int i = users.Length - 1; i >= 0 ; i-- ) {
                if ( string.IsNullOrWhiteSpace( users[ i ] ) )
                    continue;

                lstUsers.Items.Add ( users [i] );
            }
        }

        private static T FindVisualChildByName<T> ( DependencyObject parent, string name ) where T : DependencyObject {
            for ( int i = 0; i < VisualTreeHelper.GetChildrenCount( parent ); i++ ) {
                var child = VisualTreeHelper.GetChild( parent, i );
                string controlName = child.GetValue( NameProperty ) as string;
                if ( controlName == name ) {
                    return child as T;
                }
                T result = FindVisualChildByName<T>( child, name );
                if ( result != null )
                    return result;
            }
            return null;
        }

        public override string ToString () {
            return "ChatWindow {" + Tag + "}";
        }


    }
}
