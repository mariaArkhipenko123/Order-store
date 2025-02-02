using Lab.Domain.MongoEntities;
using MongoDB.Bson;

namespace Lab.Application.Interfaces.Mongo
{
    public interface IMetaDataHandler
    {
        Task CreateMetaData(MetaData metaData);
        Task DeleteMetaData(ObjectId id);
        Task<MetaData> GetMetaDataById(ObjectId id);
        Task UpdateMetaData(MetaData metaData);
    }
}