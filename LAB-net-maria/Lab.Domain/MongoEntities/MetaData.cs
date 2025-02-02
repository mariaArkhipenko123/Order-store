using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Lab.Domain.Entities;

namespace Lab.Domain.MongoEntities
{
    public class MetaData
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonIgnore]
        public UserAccess UserAccess { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid UserAccessId { get; set; }

    }
}
