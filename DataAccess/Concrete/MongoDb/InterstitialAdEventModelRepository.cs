using Core.DataAccess.MongoDb.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDb.Context;
using Entities.Concrete;

namespace DataAccess.Concrete.MongoDb
{
    public class InterstitialAdEventModelRepository : MongoDbRepositoryBase<InterstitialAdEventModel>,
        IInterstitialAdEventModelRepository
    {
        public InterstitialAdEventModelRepository(MongoDbContextBase mongoDbContext, string collectionName) : base(
            mongoDbContext.MongoConnectionSettings, collectionName)
        {
        }
    }
}