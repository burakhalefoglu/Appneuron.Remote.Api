using Cassandra.Mapping;
using Core.DataAccess.Cassandra;
using DataAccess.Abstract;
using DataAccess.Concrete.Cassandra.TableMappers;
using Entities.Concrete;

namespace DataAccess.Concrete.Cassandra;

public class CassAdvStrategyRepository : CassandraRepositoryBase<AdvStrategy>, IAdvStrategyRepository
{
    public CassAdvStrategyRepository() : base(MappingConfiguration.Global.Define<AdvStrategyMappers>())
    {
    }
}