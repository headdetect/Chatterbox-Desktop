using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Chatterbox.Gui.Converters;

namespace Chatterbox.Gui.Controls
{

    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow
    {
        public static event EventHandler<MessageEventArgs> MessageSent;

        public string Room { get; set; }

        public ChatWindow()
        {
            InitializeComponent();
#if DESIGN
            AddItem( "This is a line message that would be inserted here of sorts", "", true );
            AddItem( "This is a line message that would be inserted here of sorts", "headdetect", true );


            AddItem( "System message goes here", "", true );

            AddItem( "http://www.youtube.com/watch?v=0tCi6kj56Dg", "headdetect", true );
#endif
        }

        public ChatWindow(string room) : this()
        {
            Room = room;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {

            string text = txtMessage.Text;
#if DESIGN
            AddItem( text, "headdetect" );
#else
            if (MessageSent != null)
                MessageSent(this, new MessageEventArgs(Room, text));
#endif


            txtMessage.Clear();
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSend_Click(null, e);
            }
        }

        public void AddItem(string message, string user = "", bool isSystem = false)
        {
            if (lstChat == null || string.IsNullOrWhiteSpace(message))
                return;

            if (!isSystem)
            {
                message = message.Replace(StringToXamlConverter.ParseKey, "\x200B§\x200B");
            }

            if (string.IsNullOrWhiteSpace(user))
            {
                message = StringToXamlConverter.ParseKey + "~#666~" + StringToXamlConverter.ParseKey + message + StringToXamlConverter.ParseKey + "~E~" +
                          StringToXamlConverter.ParseKey;
            }

            lstChat.Items.Add(new ChatItem(user, message));
        }

        internal void SetUsers(string[] users)
        {
            lstUsers.Items.Clear();
            for (int i = users.Length - 1; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(users[i]))
                    continue;

                lstUsers.Items.Add(users[i]);
            }
        }

        private static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                T result = FindVisualChildByName<T>(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }

        public override string ToString()
        {
            return "ChatWindow {" + Tag + "}";
        }


        public class MessageEventArgs : EventArgs
        {
            public string Message { get; set; }
            public string Room { get; set; }

            public MessageEventArgs(string room,  string message)
            {
                Message = message;
                Room = room;
            }

        }
    }
}
