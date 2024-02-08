using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace A.Contracts.Entities
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("passwordHash")]
        public byte[] PasswordHash { get; set; }

        [BsonElement("passwordSalt")]
        public byte[] PasswordSalt { get; set; }

        [BsonElement("role")]
        public string Role { get; set; } = "student";
    }
}
