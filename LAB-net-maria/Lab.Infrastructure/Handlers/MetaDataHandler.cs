using Lab.Application.Interfaces.Mongo;
using Lab.Domain.MongoEntities;
using Lab.Infrastructure.Context;
using MongoDB.Bson;
using MongoDB.Driver;


namespace Lab.Infrastructure.Handlers
{
    public class MetaDataHandler : IMetaDataHandler
    {
        private readonly MongoDbContext _mongoDbContext;

        public MetaDataHandler(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }
        public async Task CreateMetaData(MetaData metaData)
        {
            await _mongoDbContext.MetaData.InsertOneAsync(metaData);
        }

        public async Task<MetaData> GetMetaDataById(ObjectId id)
        {
            var filter = Builders<MetaData>.Filter.Eq(m => m.Id, id);
            return await _mongoDbContext.MetaData.Find(filter).FirstOrDefaultAsync();
        }
        public async Task UpdateMetaData(MetaData metaData)
        {
            var filter = Builders<MetaData>.Filter.Eq(m => m.Id, metaData.Id);
            await _mongoDbContext.MetaData.ReplaceOneAsync(filter, metaData);
        }
        public async Task DeleteMetaData(ObjectId id)
        {
            var filter = Builders<MetaData>.Filter.Eq(m => m.Id, id);
            await _mongoDbContext.MetaData.DeleteOneAsync(filter);
        }

        // other metods
    }
}
