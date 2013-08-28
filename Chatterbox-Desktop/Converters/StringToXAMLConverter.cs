using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml;
using Chatterbox.Gui.Utils;

namespace Chatterbox.Gui.Converters {

    /// <summary>
    /// Converts a string containing valid XAML into WPF objects.
    /// </summary>
    [ValueConversion( typeof( string ), typeof( object ) )]
    public sealed class StringToXamlConverter : IValueConverter {

        /// <summary>
        /// Cant use escape codes. Cant use white space. So I will steal/use the color code char from Minecraft
        /// </summary>
        public const string ParseKey = "§";

        static StringToXamlConverter () {
            Assembly.Load( "Chatterbox.Gui" );
        }

        /// <summary>
        /// Converts a string containing valid XAML into WPF objects.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>A WPF object.</returns>
        public object Convert ( object value, Type targetType, object parameter, CultureInfo culture ) {
            string input = value as string;
            if ( input != null ) {
                string escapedXml = SecurityElement.Escape( input );

                if ( escapedXml == null )
                    return string.Empty;

                string escaped = escapedXml;

                //TODO: Add escapes for the following types:
                //      Links
                //      Color Texts
                //      Search Options


                escaped = ResolveLinks( escaped );
                escaped = ResolveEmotes( escaped );
                escaped = ResolveColors( escaped );



                string wrappedInput = string.Format(
                    "<TextBlock  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Name=\"lblMessage\" FontFamily=\"Segoe UI\" TextWrapping=\"Wrap\" Padding=\"10, 10, 0, 10\" VerticalAlignment=\"Stretch\">{0}</TextBlock>", escaped );


                using ( var stringReader = new StringReader( wrappedInput ) ) {
                    using ( XmlReader xmlReader = XmlReader.Create( stringReader ) ) {
                        TextBlock block;
                        try {
                            block = XamlReader.Load( xmlReader ) as TextBlock;
                        }
                        catch {
                            return new TextBlock( new Run() { Foreground = Brushes.Red } ) { Text = "Error getting message" };
                        }

                        if ( block == null )
                            return null;

                        foreach (var lnk in from line in block.Inlines where line.GetType() == typeof( Hyperlink ) select line as Hyperlink)
                        {
                            if ( lnk == null ) {
                                throw new Exception( "Not sure what happened here exactly..." );
                            }

                            lnk.RequestNavigate += Hyperlink_RequestNavigateEvent;
                        }

                        return block;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Converts WPF framework objects into a XAML string.
        /// </summary>
        /// <param name="value">The WPF Famework object to convert.</param>
        /// <param name="targetType">This parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">This parameter is not used.</param>
        /// <returns>A string containg XAML.</returns>
        public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture ) {
            throw new NotImplementedException( "This converter cannot be used in two-way binding." );
        }

        private static void Hyperlink_RequestNavigateEvent ( object sender, RequestNavigateEventArgs e ) {
            try {
                Process.Start( e.Uri.AbsoluteUri );
            }
            catch {
                MessageBox.Show( "Unable to open link", "sorry :/" );
            }
            e.Handled = true;
        }


        #region LinkDetection

        private static readonly Regex LinkRegex = new Regex( "((http://|https://|ftp://|mailto://|www\\.)([A-Z0-9.-:]{1,})\\.[0-9A-Z?\\,;~&#=\\+\\%\\-_\\./]{2,})", RegexOptions.Compiled | RegexOptions.IgnoreCase );
        private const string link = "<Hyperlink NavigateUri=\"{0}{1}\">{2}</Hyperlink>";
        private const string linkImage = "<LineBreak/><AsyncImage xmlns=\"http://headdetect.com/chatterbox/gui/controls\" Url=\"{0}\" StretchDirection=\"DownOnly\" MaxWidth=\"250\" MaxHeight=\"150\" />";

        public static string ResolveLinks ( string body ) {
            if ( string.IsNullOrEmpty( body ) )
                return body;

            MatchCollection collection = LinkRegex.Matches( body );

            foreach ( Match match in collection ) {

                //So if the match is a picture AND its the last match in the list. LOAD IT PLS

                if ( Regex.IsMatch( match.Value, @"([^\s]+(\.(.*)(jpg|png|gif|bmp))$)" ) && body.EndsWith( match.Value ) ) {

                    body = body.Replace( match.Value,
                                          match.Value.StartsWith( "www." )
                                              ? string.Format( link, "http://", match.Value, ShortenUrl( match.Value, 50 ) + string.Format( linkImage, match.Value ) )
                                              : string.Format( link, string.Empty, match.Value, ShortenUrl( match.Value, 50 ) + string.Format( linkImage, match.Value ) ) );

                }
                else {
                    body = body.Replace( match.Value,
                                          match.Value.StartsWith( "www." )
                                              ? string.Format( link, "http://", match.Value, ShortenUrl( match.Value, 50 ) )
                                              : string.Format( link, string.Empty, match.Value, ShortenUrl( match.Value, 50 ) ) );
                }
            }

            return body;
        }

        private static string ShortenUrl ( string url, int max ) {
            if ( url.Length <= max )
                return url;

            // Remove the protocal
            int startIndex = url.IndexOf( "://", StringComparison.Ordinal );
            if ( startIndex > -1 )
                url = url.Substring( startIndex + 3 );

            if ( url.Length <= max )
                return url;

            // Remove the folder structure
            int firstIndex = url.IndexOf( "/", StringComparison.Ordinal ) + 1;
            int lastIndex = url.LastIndexOf( "/", StringComparison.Ordinal );
            if ( firstIndex < lastIndex )
                url = url.Replace( url.Substring( firstIndex, lastIndex - firstIndex ), "..." );

            if ( url.Length <= max )
                return url;

            // Remove URL parameters
            int queryIndex = url.IndexOf( "?", StringComparison.Ordinal );
            if ( queryIndex > -1 )
                url = url.Substring( 0, queryIndex );

            if ( url.Length <= max )
                return url;

            // Remove URL fragment
            int fragmentIndex = url.IndexOf( "#", StringComparison.Ordinal );
            if ( fragmentIndex > -1 )
                url = url.Substring( 0, fragmentIndex );

            if ( url.Length <= max )
                return url;

            // Shorten page
            firstIndex = url.LastIndexOf( "/", StringComparison.Ordinal ) + 1;
            lastIndex = url.LastIndexOf( ".", StringComparison.Ordinal );
            if (lastIndex - firstIndex <= 10) return url;
            string page = url.Substring( firstIndex, lastIndex - firstIndex );
            int length = url.Length - max + 3;
            url = url.Replace( page, "..." + page.Substring( length ) );

            return url;
        }

        #endregion

        #region ColorDetection

        private static readonly Regex colorRegex = new Regex( "\\" + ParseKey + @"~#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[A-Fa-f0-9]{8})~\" + ParseKey + @"(.*?)\" + ParseKey + @"~E~\" + ParseKey, RegexOptions.Multiline | RegexOptions.Compiled );
        private const string context = "<Run Foreground=\"{0}\">{1}</Run>";

        public static string ResolveColors ( string body ) {
            if ( string.IsNullOrEmpty( body ) )
                return body;

            foreach ( Match match in colorRegex.Matches( body ) ) {
                string cutColor = match.Value.Substring( ParseKey.Length + 1, match.Value.IndexOf( "~" + ParseKey, StringComparison.Ordinal ) - ParseKey.Length - 1 );
                string message = Regex.Replace( match.Value, "\\" + ParseKey + @"~#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[A-Fa-f0-9]{8})~\" + ParseKey, string.Empty ).Replace( ParseKey + "~E~" + ParseKey, string.Empty );

                body = body.Replace( match.Value, string.Format( context, cutColor, message ) );

            }

            return body;
        }

        #endregion

        #region EmotoconDetection

        private static readonly Regex emoteRegex = new Regex( "\\([a-zA-Z]{1,}\\)", RegexOptions.Compiled );

        private const string emoteTemplate =
            "<Image Source=\"{0}\" StretchDirection=\"DownOnly\" MaxWidth=\"60\" MaxHeight=\"25\" />";

        public static string ResolveEmotes ( string body ) {
            if ( string.IsNullOrEmpty( body ) )
                return body;

            MatchCollection collection = emoteRegex.Matches( body );

            foreach ( Match match in collection ) {
                string emote = match.Value.Substring( 1, match.Value.Length - 2 );
                string stuff = Emotocon.GetImage( emote );
                if ( stuff != string.Empty ) {
                    body = body.Replace( match.Value, string.Format( emoteTemplate, "/Chatterbox.Gui;component/Emotes/" + stuff ) );
                }

            }

            return body;
        }

        #endregion
    }
}
