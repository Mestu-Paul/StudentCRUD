using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1.BOL.Entities
{

    [BsonIgnoreExtraElements]
    public class Students
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("department")]
        public string Department { get; set; }

        [BsonElement("session")]
        public string Session {  get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }
    }
}
