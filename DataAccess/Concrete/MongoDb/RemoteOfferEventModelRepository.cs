using Core.DataAccess.MongoDb.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDb.Context;
using Entities.Concrete;

namespace DataAccess.Concrete.MongoDb
{
    public class RemoteOfferEventModelRepository : MongoDbRepositoryBase<RemoteOfferEventModel>,
        IRemoteOfferEventModelRepository
    {
        public RemoteOfferEventModelRepository(MongoDbContextBase mongoDbContext, string collectionName) : base(
            mongoDbContext.MongoConnectionSettings, collectionName)
        {
        }
    }
}