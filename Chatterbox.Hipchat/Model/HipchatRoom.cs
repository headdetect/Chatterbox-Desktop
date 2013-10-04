using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chatterbox.Hipchat.Model
{
    public class HipchatRoom
    {
        public string Name { get; set; }
        public string RoomID { get; set; }

        internal HipchatRoom(string name, string roomID)
        {
            Name = name;
            RoomID = roomID;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
