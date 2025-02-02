using Lab.Domain.MongoEntities;
using MongoDB.Bson;

namespace Lab.Application.Interfaces.Mongo
{
    public interface IAuditLogHandler
    {
        Task CreateAuditLog(AuditLog log);
        Task DeleteAuditLog(ObjectId id);
        Task<AuditLog> GetAuditLogById(ObjectId id);
        Task UpdateAuditLog(AuditLog auditLog);
    }
}