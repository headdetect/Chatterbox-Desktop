using System;

namespace Chatterbox {

    public class ChatItem {

        public string Username { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }

        public ChatItem ()
            : this( string.Empty, string.Empty ) {
        }

        public ChatItem ( string message )
            : this( string.Empty, message ) {
        }

        public ChatItem ( string username, string message )
            : this( username, message, DateTime.Now ) {
        }

        public ChatItem ( string username, string message, DateTime time ) {
            Username = username;
            Message = message;
            Time = time;
        }

    }
}