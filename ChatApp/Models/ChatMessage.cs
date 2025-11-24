using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ChatApp.Models
{
    public class ChatMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Room { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ChatGroup
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Admin")]
        public string Admin { get; set; }  

        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; } 

        [BsonElement("Members")]
        public List<string> Members { get; set; } = new List<string>();
    }

}