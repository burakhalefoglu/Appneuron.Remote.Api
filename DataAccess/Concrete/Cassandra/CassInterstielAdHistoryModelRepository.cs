using Core.DataAccess.Cassandra;
using DataAccess.Abstract;
using DataAccess.Concrete.Cassandra.Contexts;
using Entities.Concrete;

namespace DataAccess.Concrete.Cassandra
{
    public class CassInterstitialAdHistoryModelRepository : CassandraRepositoryBase<InterstitialAdHistoryModel>,
        IInterstielAdHistoryModelRepository
    {
        public CassInterstitialAdHistoryModelRepository(CassandraContextBase cassandraContexts, string tableQuery) : base(
            cassandraContexts.CassandraConnectionSettings, tableQuery)
        {
        }
    }
}