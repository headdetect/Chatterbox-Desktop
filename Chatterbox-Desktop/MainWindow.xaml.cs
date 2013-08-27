using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Chatterbox.Gui.Controls;
using Meebey.SmartIrc4net;

namespace Chatterbox.Gui {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        private IrcClient client;

        public ChatWindow ActiveChatWindow { get; set; }


        public MainWindow () {
            InitializeComponent();

        }

        #region UI Events

        private void Window_Loaded ( object sender, RoutedEventArgs e ) {


            client = IrcController.Client;
            client.OnConnecting += client_OnConnecting;
            client.OnConnected += client_OnConnected;
            client.OnConnectionError += client_OnConnectionError;
            client.OnChannelMessage += client_OnChannelMessage;
            client.OnError += client_OnError;
            client.OnErrorMessage += client_OnErrorMessage;
            client.OnChannelNotice += client_OnChannelNotice;
            client.OnChannelAction += client_OnChannelAction;
            client.OnNames +=  client_OnNames ;

#if !DESIGN
            IrcController.Connect();
#endif


        }




        #endregion


        private bool Reinvoke<T> ( Action<object, T> method, object sender, T e ) {
            try {
                Boolean boold = (Boolean) sender;

                if ( !boold ) {
                    sender = true;
                }
            }
            catch ( InvalidCastException ) {
                sender = false;
                Dispatcher.BeginInvoke( method, sender, e );
            }

            return (bool) sender;
        }

        private void tbControl_SelectionChanged ( object sender, SelectionChangedEventArgs e ) {
            e.Handled = true;

            if ( !IsLoaded )
                return;

            if ( e.OriginalSource is TabControl ) {
                TabControl sawse = e.Source as TabControl;
                if ( sawse != null ) {
                    TabItem itm = sawse.SelectedItem as TabItem;

                    if ( itm == null )
                        return;

                    ActiveChatWindow = itm.Content as ChatWindow;
                }
            }


        }



        private static T FindVisualChildByName<T> ( DependencyObject parent, string name ) where T : DependencyObject {
            for ( int i = 0; i < VisualTreeHelper.GetChildrenCount( parent ); i++ ) {
                var child = VisualTreeHelper.GetChild( parent, i );
                string controlName = child.GetValue( NameProperty ) as string;
                if ( controlName == name && child is T ) {
                    return child as T;
                }
                T result = FindVisualChildByName<T>( child, name );
                if ( result != null )
                    return result;
            }
            return null;
        }

        private static T FindVisualChildByType<T> ( DependencyObject parent ) where T : DependencyObject {
            for ( int i = 0; i < VisualTreeHelper.GetChildrenCount( parent ); i++ ) {
                var child = VisualTreeHelper.GetChild( parent, i );
                if ( child is T ) {
                    return child as T;
                }
                T result = FindVisualChildByType<T>( child );
                if ( result != null )
                    return result;
            }
            return null;
        }

        internal void AddItem ( string message, string nick, string channel ) {
            for ( int i = 1; i < tbControl.Items.Count; i++ ) {
                var tb = tbControl.ItemContainerGenerator.ContainerFromIndex( i ) as TabItem;

                if ( tb == null )
                    continue;

                ChatWindow window = tb.Content as ChatWindow;

                if ( window == null )
                    continue;

                if ( window.Channel == channel ) {
                    window.AddItem( message, nick );
                }
            }
        }

        private void AddUsers ( string[] users, string channel ) {
            for ( int i = 1; i < tbControl.Items.Count; i++ ) {
                var tb = tbControl.ItemContainerGenerator.ContainerFromIndex( i ) as TabItem;

                if ( tb == null )
                    continue;

                ChatWindow window = tb.Content as ChatWindow;

                if ( window == null )
                    continue;

                if ( window.Channel == channel ) {
                    window.AddUser( users );
                }
            }
        }

    }




}
