using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chatterbox.Gui.Controls;
using Chatterbox.Plugins;
using Elysium;

namespace Chatterbox.Gui
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ChatWindow ActiveChatWindow { get; set; }

        public List<ChatPlugin> Plugins;

        public MainWindow()
        {
            Plugins = PluginLoader.Plugins;
            ChatWindow.MessageSent += ChatWindow_MessageSent;
            InitializeComponent();
        }

        void ChatWindow_MessageSent(object sender, ChatWindow.MessageEventArgs e)
        {
            foreach (var handle in Plugins)
                handle.OnMessage(e.Room, e.Message);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var notUsed in Plugins.Where(handle => !handle.OnLoad(this)))
            {
                // TODO: Elysium has bug. Can't use this.Close(); 
                Environment.Exit(0);
            }
        }

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

        public void SetUsers(string[] users, string room)
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
                        window.SetUsers(users);
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

        public void SetLobbyRoom(UserControl control)
        {
            tbLobby.Content = control;
        }

    }




}
