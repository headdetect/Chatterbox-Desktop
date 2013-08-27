using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Elysium;

namespace Chatterbox.Gui
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
        }

    }
}
