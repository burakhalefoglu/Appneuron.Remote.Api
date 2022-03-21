using Cassandra.Mapping;
using Core.DataAccess.Cassandra.Configurations;
using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Concrete.Cassandra.TableMappers;

public class AdvStrategyMappers: Mappings
{
    public AdvStrategyMappers()
    {
        var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        var cassandraConnectionSettings = 
            configuration.GetSection("CassandraConnectionSettings").Get<CassandraConnectionSettings>();
        For<AdvStrategy>()
            .TableName("adv_strategy_models")
            .KeyspaceName(cassandraConnectionSettings.Keyspace)
            .PartitionKey("name", "project_id", "version", "status")
            .ClusteringKey(new Tuple<string, SortOrder>("created_at", SortOrder.Descending))
            .Column(u => u.Id, cm => cm.WithName("id").WithDbType(typeof(long)))
            .Column(u => u.Version, cm => cm.WithName("version").WithDbType(typeof(string)))
            .Column(u => u.ProjectId, cm => cm.WithName("project_id").WithDbType(typeof(long)))
            .Column(u => u.Name, cm => cm.WithName("name").WithDbType(typeof(string)))
            .Column(u => u.StrategyCount, cm => cm.WithName("strategy_count").WithDbType(typeof(float)))
            .Column(u => u.StrategyName, cm => cm.WithName("strategy_name").WithDbType(typeof(string)))
            .Column(u => u.Status, cm => cm.WithName("status").WithDbType(typeof(bool)))
            .Column(u => u.CreatedAt, cm => cm.WithName("created_at").WithDbType(typeof(DateTimeOffset)));

    }
}