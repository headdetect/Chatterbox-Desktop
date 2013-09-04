using System;
using System.Text;
using System.Threading;
using Meebey.SmartIrc4net;

namespace Chatterbox.Irc
{
    public class IrcController
    {

        private static IrcClient _irc;

        public static bool Connected { get; set; }

        public static IrcClient Client
        {
            get
            {
                return _irc ?? (_irc = new IrcClient
                {
                    Encoding = Encoding.UTF8,
                    ActiveChannelSyncing = true,
                });
            }
        }

        public static void Connect()
        {
            new Thread(() =>
            {
                if (_irc == null)
                    return;

                _irc.Connect("irc.uclcommander.net", 6667);
                _irc.Login("BotDeHeaddetect", "MahNegro");
                _irc.RfcJoin("#bots");

                _irc.Listen();
            }).Start();
        }

        public static void Stop()
        {
            if (_irc == null)
            {
                throw new MethodAccessException("This method can only be called after \"Connect()\" and called once. Unless \"Connect()\" is called.");
            }

            _irc.Disconnect();
            _irc = null;
        }

    }

}
