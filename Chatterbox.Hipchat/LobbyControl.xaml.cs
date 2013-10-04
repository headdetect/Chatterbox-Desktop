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

        public class RoomJoinEventArgs : EventArgs
        {
            public HipchatRoom HipchatRoom { get; set; }

            public RoomJoinEventArgs(HipchatRoom room)
            {
                HipchatRoom = room;
            }
        }
    }
}
