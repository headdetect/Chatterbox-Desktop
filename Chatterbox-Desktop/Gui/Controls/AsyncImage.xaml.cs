using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Chatterbox.Gui.Controls
{
    /// <summary>
    /// Interaction logic for AsyncImage.xaml
    /// </summary>
    public partial class AsyncImage
    {
        public AsyncImage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The URL property.
        /// </summary>
        public static DependencyProperty UrlProperty = DependencyProperty.Register("UrlProperty", typeof(string), typeof(AsyncImage), new PropertyMetadata(OnPropertyChangedCallback));

        /// <summary>
        /// Gets or sets the URL of the image.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string url = (string)e.NewValue;
            if (url == null) return;
            AsyncImage img = d as AsyncImage;
            if (img == null) return;
            img.Source = null;
            WebClient client = new WebClient();
            client.DownloadDataCompleted += OnImageReady;

            //Muddafukka is slow.

            ThreadPool.QueueUserWorkItem(a =>
            {
                try
                {
                    client.DownloadDataAsync(new Uri(url), img);
                    client.Dispose();
                }
                catch
                {
                    //TODO: Show no image thing.
                }
            });
        }

        static void OnImageReady(object sender, DownloadDataCompletedEventArgs e)
        {

            AsyncImage d = e.UserState as AsyncImage;

            if (d == null)
                return;

            if (e.Error != null)
            {
                d.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(a =>
                {
                    d.Source = null;
                    d.Visibility = Visibility.Collapsed;
                    return null;
                }), null);
                return;
            }

            byte[] data = e.Result;
            if (data == null)
                return;

            d.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(a =>
            {
                MemoryStream stream = new MemoryStream(data);
                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = stream;
                imageSource.EndInit();
                d.Source = imageSource;

                return null;
            }), null);
        }

    }
}
