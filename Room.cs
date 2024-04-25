using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class Room
    {
       
            public int RoomId { get; set; }
            public string Name { get; set; }

            public List<Message> Messages { get; set; }

            public Room(int roomId, string name)
            {
                RoomId = roomId;
                Name = name;
                Messages = new List<Message>();
            }
        
    }
}
