using System;

namespace Chatterbox.Gui
{

    public class ChatItem
    {

        public string Username { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }

        public string Highlight { get; set; }

        public ChatItem()
            : this(string.Empty, string.Empty)
        {
        }

        public ChatItem(string message, string color = ChatColor.System)
            : this(string.Empty, message, color)
        {
        }

        public ChatItem(string username, string message, string color = ChatColor.Normal)
            : this(username, message, DateTime.Now, color)
        {
        }

        public ChatItem(string username, string message, DateTime time, string color = ChatColor.Normal)
        {
            Username = username;
            Message = message;
            Time = time;

            Highlight = color;
        }

    }

    public static class ChatColor
    {
        public const string Blue = "#00F";
        public const string Red = "#00F";
        public const string Green = "#00F";
        public const string White = "#00F";
        public const string Orange = "#00F";
        public const string Purple = "#00F";
        public const string Yellow = "#00F";
        public const string Gray = "#00F";
        public const string Self = Blue;
        public const string System = Gray;
        public const string Normal = White;
    }
}