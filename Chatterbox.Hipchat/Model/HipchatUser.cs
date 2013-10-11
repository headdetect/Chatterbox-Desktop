using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;
using agsXMPP.protocol.client;

namespace Chatterbox.Hipchat.Model
{
    public class HipchatUser
    {
        public Jid Jid { get; set; }

        public string Name { get { return Jid.User; } }

        public ShowType Status { get { return ShowType.NONE; } }

        internal HipchatUser(Jid jid)
        {
            Jid = jid;
        }
    }
}
