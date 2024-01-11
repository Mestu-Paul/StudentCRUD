using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.Contracts.Models
{
    [BsonIgnoreExtraElements]
    public class Student
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

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("session")]
        public string Session { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("blood_group")]
        public string BloodGroup { get; set; }

        [BsonElement("last_donated_at")]
        public string LastDonatedAt { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }
    }
}
