
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace A.Contracts.Entities
{
    [BsonIgnoreExtraElements]
    public class Teacher
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("teacher_id")]
        public string TeacherId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("joinedAt")]
        public DateOnly? JoinedAt { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("education")]
        public string Education { get; set; }

        [BsonElement("research")]
        public string Research { get; set; }
    }
}
