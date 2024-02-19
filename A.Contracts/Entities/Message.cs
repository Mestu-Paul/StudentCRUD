using A.Contracts.DataTransferObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace A.Contracts.Entities
{
    public class Message
    {
        public Message() { }
        public Message(MessageDTO messageDto)
        {
            this.Content = messageDto.Content;
            this.SenderUsername = messageDto.SenderUsername;
            this.RecipientUsername = messageDto.RecipientUsername;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    }
}
