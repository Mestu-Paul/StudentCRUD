using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace A.Contracts.Models
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

    public class UserForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
