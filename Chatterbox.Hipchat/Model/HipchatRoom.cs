using System.Collections.Generic;
using System.Windows.Documents;
using agsXMPP;

namespace Chatterbox.Hipchat.Model
{
    public class HipchatRoom
    {
        public string Name { get; set; }
        public Jid RoomId { get; set; }

        public Jid RoomAdmin { get; set; }
        public bool Archived { get; set; }

        public List<string> Users = new List<string>();

        public HipchatRoom(string name, string roomId)
        {
            Name = name;
            RoomId = new Jid(roomId);
        }

        public HipchatRoom(string name, Jid roomId)
        {
            Name = name;
            RoomId = roomId;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
