using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chatterbox.Gui
{
    public interface IChatHandler
    {
        /// <summary>
        /// Called when the chat handler is ready to go.
        /// </summary>
        void OnLoad(MainWindow window);

        /// <summary>
        /// Called when an attempt to join a room is made
        /// </summary>
        /// <param name="room">room user is attempting to join</param>
        /// <returns>True if user can join room</returns>
        bool OnJoinRoom(string room);

        /// <summary>
        /// Called when an attempt to send a message is made
        /// </summary>
        /// <param name="message">message user is attempting to send</param>
        /// <param name="room"></param>
        /// <returns>true if user can send message</returns>
        bool OnMessage(string room, string message);

    }
}
