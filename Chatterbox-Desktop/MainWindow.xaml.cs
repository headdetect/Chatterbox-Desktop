using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chatterbox.Gui.Controls;

namespace Chatterbox.Gui
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ChatWindow ActiveChatWindow { get; set; }

        public static IChatHandler ChatHandler { get; set; }

        public MainWindow()
        {
            ChatWindow.MessageSent += ChatWindow_MessageSent;
            InitializeComponent();
        }

        void ChatWindow_MessageSent(object sender, ChatWindow.MessageEventArgs e)
        {
            if (ChatHandler == null)
                throw new InvalidOperationException("A ChatHandler must be set");

            ChatHandler.OnMessage(e.Room, e.Message);
        }

        #region UI Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ChatHandler == null)
                throw new InvalidOperationException("A ChatHandler must be set");

            ChatHandler.OnLoad(this);
        }


        #endregion

        private void tbControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;

            if (!IsLoaded)
                return;

            if (!(e.OriginalSource is TabControl)) return;
            TabControl sawse = e.Source as TabControl;
            if (sawse == null) return;
            TabItem itm = sawse.SelectedItem as TabItem;

            if (itm == null)
                return;

            ActiveChatWindow = itm.Content as ChatWindow;
        }



        private static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(NameProperty) as string;
                if (controlName == name && child is T)
                {
                    return child as T;
                }
                T result = FindVisualChildByName<T>(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }

        private static T FindVisualChildByType<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    return child as T;
                }
                T result = FindVisualChildByType<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        public void AddItem(string message, string user, string room)
        {
            Dispatcher.Invoke(new Action(() =>
            {

                for (int i = 1; i < tbControl.Items.Count; i++)
                {
                    var tb = tbControl.ItemContainerGenerator.ContainerFromIndex(i) as TabItem;

                    if (tb == null)
                        continue;

                    ChatWindow window = tb.Content as ChatWindow;

                    if (window == null)
                        continue;

                    if (window.Room == room)
                    {
                        window.AddItem(message, user);
                    }
                }

            }));
        }

        public void AddUsers(string[] users, string room)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                for (int i = 1; i < tbControl.Items.Count; i++)
                {
                    var tb = tbControl.ItemContainerGenerator.ContainerFromIndex(i) as TabItem;

                    if (tb == null)
                        continue;

                    ChatWindow window = tb.Content as ChatWindow;

                    if (window == null)
                        continue;

                    if (window.Room == room)
                    {
                        window.AddUser(users);
                    }
                }
            }));
        }

        public void CreateRoom(string room)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                for (int i = 1; i < tbControl.Items.Count; i++)
                {
                    var tb = tbControl.ItemContainerGenerator.ContainerFromIndex(i) as TabItem;

                    if (tb == null)
                        continue;

                    ChatWindow window = tb.Content as ChatWindow;

                    if (window == null)
                        continue;

                    if (window.Room == room)
                    {
                        return;
                    }
                }

                var tab = new TabItem { Content = new ChatWindow(room), Header = room };
                tbControl.Items.Add(tab);
            }));
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Create window that asks for new room
            //TODO: use chat handler to check to see if can join
            //TODO: if can join, join; else error
        }

    }




}
