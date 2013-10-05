using System;
using System.Drawing;

namespace Chatterbox.Gui
{

    internal class ChatItem
    {

        public string Username { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }

        public SolidBrush Highlight { get; set; }

        public SolidBrush HighlightDark
        {
            get
            {
                Color clr = Color.FromArgb
                (
                    0xFF,
                    Math.Min(Highlight.Color.R - 40, 0),
                    Math.Min(Highlight.Color.G - 40, 0),
                    Math.Min(Highlight.Color.B - 40, 0)
                );

                return new SolidBrush(clr);
            }
        }

        public ChatItem()
            : this(string.Empty, string.Empty)
        {
        }

        public ChatItem(string message)
            : this(string.Empty, message, ChatColor.Normal)
        {
        }

        public ChatItem(string username, string message)
            : this(username, message, ChatColor.Normal)
        {
        }

        public ChatItem(string username, string message, SolidBrush color)
            : this(username, message, DateTime.Now, color)
        {
        }

        public ChatItem(string username, string message, DateTime time, SolidBrush color)
        {
            Username = username;
            Message = message;
            Time = time;

            Highlight = color;
        }

    }

    public static class ChatColor
    {
        public static SolidBrush Blue = new SolidBrush(Color.FromArgb(0xCB, 0xEE, 0xF2));
        public static SolidBrush Red = new SolidBrush(Color.FromArgb(0xF2, 0xD8, 0xD8));
        public static SolidBrush Green = new SolidBrush(Color.FromArgb(0xDB, 0xF2, 0xD8));
        public static SolidBrush White = new SolidBrush(Color.FromArgb(0xFF, 0xFF, 0xFF));
        public static SolidBrush Orange = new SolidBrush(Color.FromArgb(0xF2, 0xDB, 0xCB));
        public static SolidBrush Purple = new SolidBrush(Color.FromArgb(0xEE, 0xCB, 0xF2));
        public static SolidBrush Yellow = new SolidBrush(Color.FromArgb(0xED, 0xF2, 0xCB));
        public static SolidBrush Gray = new SolidBrush(Color.FromArgb(0xD9, 0xD9, 0xD9));
        public static SolidBrush Self = Blue;
        public static SolidBrush System = Gray;
        public static SolidBrush Normal = White;
    }
}