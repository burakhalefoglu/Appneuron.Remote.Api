using Cassandra.Mapping;
using Core.DataAccess.Cassandra;
using DataAccess.Abstract;
using DataAccess.Concrete.Cassandra.TableMappers;
using Entities.Concrete;

namespace DataAccess.Concrete.Cassandra;
public class CassRemoteOfferProductModelRepository : CassandraRepositoryBase<RemoteOfferProductModel>, IRemoteOfferProductModelRepository
    {
        public CassRemoteOfferProductModelRepository() : base(MappingConfiguration.Global.Define<RemoteOfferProductModelMappers>())
        {
        }
    }
 