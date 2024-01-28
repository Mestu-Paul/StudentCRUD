using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace A.Contracts.Models
{
    [BsonIgnoreExtraElements]
    public class Student : UpdateStudent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("created_at")]
        public string CreatedAt { get; set; }

        [BsonElement("last_updated_at")]
        public string LastUpdatedAt { get; set; }

        [BsonElement("student_id")]
        public string StudentId { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("blood_group")]
        public string BloodGroup { get; set; }
    }
}
