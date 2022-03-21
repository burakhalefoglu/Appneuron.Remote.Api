using Cassandra.Mapping;
using Core.DataAccess.Cassandra;
using DataAccess.Abstract;
using DataAccess.Concrete.Cassandra.TableMappers;
using Entities.Concrete;

namespace DataAccess.Concrete.Cassandra;
public class CassInterstitialAdModelRepository  : CassandraRepositoryBase<InterstitialAdModel>, IInterstielAdModelRepository
    {
        public CassInterstitialAdModelRepository() : base(MappingConfiguration.Global.Define<InterstitialAdModelMappers>())
        {
        }
    }
