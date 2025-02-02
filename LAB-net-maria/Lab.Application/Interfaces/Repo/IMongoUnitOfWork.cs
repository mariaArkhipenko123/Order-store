using Lab.Domain.MongoEntities;
using MongoDB.Driver;

namespace Lab.Application.Interfaces
{
    public interface IMongoUnitOfWork
    {
        IMongoCollection<AuditLog> AuditLogs { get; }
        IMongoCollection<MetaData> MetaData { get; }
        Task SaveChangesAsync();
    }
}