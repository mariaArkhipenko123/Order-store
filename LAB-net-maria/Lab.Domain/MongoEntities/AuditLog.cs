using Lab.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lab.Domain.MongoEntities
{
    public class AuditLog
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }
        public string Action { get; set; }
        public string Time { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified).ToString("O");

        [BsonIgnore]
        public User User { get; set; }
    }
    public enum AuditLogAction
    {
        Register,
        Login,
        LoginWithOAuth,
        LoginWithFirebase
    }
}
