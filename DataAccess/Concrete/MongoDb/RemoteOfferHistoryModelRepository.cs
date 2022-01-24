using Core.DataAccess.MongoDb.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDb.Context;
using Entities.Concrete;

namespace DataAccess.Concrete.MongoDb
{
    public class RemoteOfferHistoryModelRepository : MongoDbRepositoryBase<RemoteOfferHistoryModel>,
        IRemoteOfferHistoryModelRepository
    {
        public RemoteOfferHistoryModelRepository(MongoDbContextBase mongoDbContext, string collectionName) : base(
            mongoDbContext.MongoConnectionSettings, collectionName)
        {
        }
    }
}