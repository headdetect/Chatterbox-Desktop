using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SKYPE4COMLib;

namespace Chatterbox.Skype
{
    /// <summary>
    /// Interaction logic for SelectContactWindow.xaml
    /// </summary>
    public partial class SelectContactWindow : Window
    {
        ObservableCollection<ContactHolder> Contacts = new ObservableCollection<ContactHolder>(); 
        private SkypeChatHandler handler;
        public SelectContactWindow(SkypeChatHandler handler)
        {
            InitializeComponent();
            this.handler = handler;
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var friends = handler._skype.Friends.Cast<User>();
            foreach (User user in friends)
            {
                //SaveSkypeAvatar(user.Handle, user.FullName + ".jpg");
                var holder = new ContactHolder {ContactDisplayName = user.DisplayName};
                Contacts.Add(holder);
            }
        }

        private void SaveSkypeAvatar(string userHandle, string Path)
        {
            if (!System.IO.Path.IsPathRooted(Path))
                throw new ArgumentException("Filename does not contain full path!", "rootedPathFileName");

            var extension = System.IO.Path.GetExtension(Path);
            if (extension != null && !extension.Equals(".jpg"))
                throw new ArgumentException("Filename does not represent jpg file!", "rootedPathFileName");

            var cmd = new Command {Command = string.Format("GET USER {0} AVATAR 1 {1}", userHandle, Path)};
            handler._skype.SendCommand(cmd);
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        class ContactHolder
        {
            public string ContactDisplayName;
            public Image ContactAvatar;
        }

        private void tbControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
