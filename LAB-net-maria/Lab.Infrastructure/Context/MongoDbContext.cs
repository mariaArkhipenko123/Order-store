using Lab.Domain.Entities;
using Lab.Domain.MongoEntities;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Lab.Infrastructure.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly MongoClient _client;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
            //mongoClientSettings.GuidRepresentation = GuidRepresentation.Standard; // Установите представление GUID  

            _client = new MongoClient(mongoClientSettings);
            _database = _client.GetDatabase(databaseName);
        }

        public IMongoCollection<MetaData> MetaData => _database.GetCollection<MetaData>("MetaData");
        public IMongoCollection<AuditLog> AuditLogs => _database.GetCollection<AuditLog>("AuditLogs");
    }
}
