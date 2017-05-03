using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Cayci.Entities
{
    public abstract class ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
