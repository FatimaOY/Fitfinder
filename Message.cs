using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitfinder
{
    public class Message
    {
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageContent { get; set; }

        public Room Room { get; set; }
        public Client Sender { get; set; }
        public Trainer Receiver { get; set; }

        public Message(
            int messageId,
            int conversationId,
            int senderId,
            int receiverId,
            string messageContent)
        {
            MessageId = messageId;
            ConversationId = conversationId;
            SenderId = senderId;
            ReceiverId = receiverId;
            MessageContent = messageContent;
        }
    }
}
