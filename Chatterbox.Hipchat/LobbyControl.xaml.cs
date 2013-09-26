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

namespace Chatterbox.Hipchat
{
    /// <summary>
    /// Interaction logic for LobbyControl.xaml
    /// </summary>
    public partial class LobbyControl
    {
        public LobbyControl()
        {
            InitializeComponent();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            if (lstRooms.SelectedIndex == -1) return;

        }
    }
}
