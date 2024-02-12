using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace A.Contracts.Entities
{
    public class Recipient
    {
        public string username { get; set; }
        public DateTime timestamp { get; set; } = DateTime.UtcNow;
    }
    public class ChatList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string username { get; set; }
        public List<Recipient> recipientUsername { get; set; }

    }
}
