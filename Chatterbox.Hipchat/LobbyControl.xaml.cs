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
using Chatterbox.Hipchat.Model;

namespace Chatterbox.Hipchat
{
    /// <summary>
    /// Interaction logic for LobbyControl.xaml
    /// </summary>
    public partial class LobbyControl
    {
        public event EventHandler<RoomJoinEventArgs> OnRoomJoin;

        public LobbyControl()
        {
            InitializeComponent();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (lstRooms.SelectedIndex == -1) return;

            HipchatRoom room = lstRooms.Items[lstRooms.SelectedIndex] as HipchatRoom;

            if (OnRoomJoin != null && room != null)
                OnRoomJoin(this, new RoomJoinEventArgs(room));
        }

        private void btnCreateRoom_Click(object sender, RoutedEventArgs e)
        {
            // HipchatChatPlugin.MucManager.CreateReservedRoom();
        }

        private void lstRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnJoin.IsEnabled = lstRooms.SelectedIndex != -1;
        }

        private void btnCreateRoom_Loaded(object sender, RoutedEventArgs e)
        {
            if (!HipchatChatPlugin.IsAdmin)
            {
                btnCreateRoom.Visibility = Visibility.Hidden;
            }
        }


        public void DoRoomCheck()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                lstRooms.Items.Clear();
                foreach (var room in HipchatChatPlugin.Rooms)
                    lstRooms.Items.Add(room);

                if (!HipchatChatPlugin.IsAdmin)
                {
                    btnCreateRoom.Visibility = Visibility.Hidden;
                }
            }));
        }

        public void DoUserCheck()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                lstUsers.Items.Clear();
                foreach (var room in HipchatChatPlugin.Users)
                    lstUsers.Items.Add(room);

                if (!HipchatChatPlugin.IsAdmin)
                {
                    btnCreateRoom.Visibility = Visibility.Hidden;
                }
            }));
        }


        public class RoomJoinEventArgs : EventArgs
        {
            public HipchatRoom HipchatRoom { get; set; }

            public RoomJoinEventArgs(HipchatRoom room)
            {
                HipchatRoom = room;
            }
        }

        private void TextBlock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            HipchatRoom room = new HipchatRoom("i", "");

            if (OnRoomJoin != null)
                OnRoomJoin(this, new RoomJoinEventArgs(room));
        }

        #region Custom Event
        private int _clickCount = 0;
        private DateTime _lastClick = DateTime.Now;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _clickCount++;

            if ((DateTime.Now - _lastClick).Milliseconds > 100)
            {
                _clickCount = 0;
            }

            if (_clickCount >= 2)
            {
                _clickCount = 0;
                TextBlock_MouseDoubleClick(sender, e);
            }

            _lastClick = DateTime.Now;
        }
        #endregion
    }
}
