using System;
using Chatterbox.Gui;

namespace Chatterbox.Plugins
{
    public abstract class ChatPlugin
    {
        public string PluginName { get; set; }

        public Version PluginVersion { get; set; }

        /// <summary>
        /// Called when the chat handler is ready to go.
        /// </summary>
        public abstract bool OnLoad(MainWindow window);


        /// <summary>
        /// Called when an attempt to send a message is made
        /// </summary>
        /// <param name="message">message user is attempting to send</param>
        /// <param name="room"></param>
        /// <returns>The new message</returns>
        public abstract MessageBlock OnMessage(string room, string message);

    }
}
