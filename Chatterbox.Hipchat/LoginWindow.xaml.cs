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
using System.Windows.Shapes;

namespace Chatterbox.Hipchat
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Username = txtUsername.Text;
            Password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                lblMessage.Text = "Please enter a username and password.";
                return;
            }

            HipchatChatPlugin.DoLogin(Username, Password);
            if (HipchatChatPlugin.LoginData == null)
            {
                lblMessage.Text = "Username or password is incorrect.";
                return;
            }
            Close();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnLogin_Click(null, null);
            }
        }
    }
}
