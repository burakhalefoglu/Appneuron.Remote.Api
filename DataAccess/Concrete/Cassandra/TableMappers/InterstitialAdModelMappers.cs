using Cassandra.Mapping;
using Core.DataAccess.Cassandra.Configurations;
using Core.Entities.Concrete;
using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Concrete.Cassandra.TableMappers;

public class InterstitialAdModelMappers: Mappings
{
    public InterstitialAdModelMappers()
    {
        var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        var cassandraConnectionSettings = 
            configuration.GetSection("CassandraConnectionSettings").Get<CassandraConnectionSettings>();
        For<InterstitialAdModel>()
            .TableName("interstitial_ad_models")
            .KeyspaceName(cassandraConnectionSettings.Keyspace)
            .PartitionKey("name", "project_id", "version", "status")
            .ClusteringKey(new Tuple<string, SortOrder>("created_at", SortOrder.Descending))
            .Column(u => u.Id, cm => cm.WithName("id").WithDbType(typeof(long)))
            .Column(u => u.ProjectId, cm => cm.WithName("project_id").WithDbType(typeof(long)))
            .Column(u => u.Name, cm => cm.WithName("name").WithDbType(typeof(string)))
            .Column(u => u.Version, cm => cm.WithName("version").WithDbType(typeof(string)))
            .Column(u => u.PlayerPercent, cm => cm.WithName("player_percent").WithDbType(typeof(int)))
            .Column(u => u.Status, cm => cm.WithName("status").WithDbType(typeof(bool)))
            .Column(u => u.Terminated, cm => cm.WithName("terminated").WithDbType(typeof(bool)))
            .Column(u => u.CreatedAt, cm => cm.WithName("created_at").WithDbType(typeof(DateTimeOffset)));

    }
}