﻿using Core.DataAccess.MongoDb.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDb.Context;
using Entities.Concrete;

namespace DataAccess.Concrete.MongoDb
{
    public class InterstielAdHistoryModelRepository : MongoDbRepositoryBase<InterstielAdHistoryModel>,
        IInterstielAdHistoryModelRepository
    {
        public InterstielAdHistoryModelRepository(MongoDbContextBase mongoDbContext, string collectionName) : base(
            mongoDbContext.MongoConnectionSettings, collectionName)
        {
        }
    }
}