using Cassandra.Mapping;
using Core.DataAccess.Cassandra;
using DataAccess.Abstract;
using DataAccess.Concrete.Cassandra.TableMappers;
using Entities.Concrete;

namespace DataAccess.Concrete.Cassandra;

public class CassRemoteOfferModelRepository : CassandraRepositoryBase<RemoteOfferModel>, IRemoteOfferModelRepository
{
    public CassRemoteOfferModelRepository() : base(MappingConfiguration.Global.Define<RemoteOfferModelMappers>())
    {
    }
}