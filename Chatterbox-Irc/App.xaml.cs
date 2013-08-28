using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Elysium;

namespace Chatterbox.Irc
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void StartupHandler(object sender, StartupEventArgs e)
        {
            this.Apply(Theme.Light, AccentBrushes.Purple, Brushes.White);

            //Load the proxy for WebClients everywhere
            ThreadPool.QueueUserWorkItem(a =>
            {
                using (var c = new WebClient())
                {
                    c.DownloadString("http://google.com");
                }
            });

            Gui.MainWindow.ChatHandler = new IrcChatHandler();
        }
    }
}
