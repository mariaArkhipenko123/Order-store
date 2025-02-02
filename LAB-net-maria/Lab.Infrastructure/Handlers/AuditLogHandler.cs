using Lab.Application.Interfaces.Mongo;
using Lab.Domain.MongoEntities;
using Lab.Infrastructure.Context;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Lab.Infrastructure.Handlers
{
    public class AuditLogHandler : IAuditLogHandler
    {
        private readonly MongoDbContext _mongoDbContext;

        public AuditLogHandler(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public async Task CreateAuditLog(AuditLog log)
        {
            await _mongoDbContext.AuditLogs.InsertOneAsync(log);
        }

        public async Task<AuditLog> GetAuditLogById(ObjectId id)
        {
            var filter = Builders<AuditLog>.Filter.Eq(m => m.Id, id);
            return await _mongoDbContext.AuditLogs.Find(filter).FirstOrDefaultAsync();
        }
        public async Task UpdateAuditLog(AuditLog auditLog)
        {
            var filter = Builders<AuditLog>.Filter.Eq(m => m.Id, auditLog.Id);
            await _mongoDbContext.AuditLogs.ReplaceOneAsync(filter, auditLog);
        }
        public async Task DeleteAuditLog(ObjectId id)
        {
            var filter = Builders<AuditLog>.Filter.Eq(m => m.Id, id);
            await _mongoDbContext.AuditLogs.DeleteOneAsync(filter);
        }
        // Other metods
    }
}
