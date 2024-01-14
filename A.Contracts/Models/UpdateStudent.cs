using MongoDB.Bson.Serialization.Attributes;

namespace A.Contracts.Models
{
    public class UpdateStudent
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("session")]
        public string Session { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("last_donated_at")]
        public string LastDonatedAt { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }
    }
}
