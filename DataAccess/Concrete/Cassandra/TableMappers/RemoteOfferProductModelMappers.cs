using Cassandra.Mapping;
using Core.DataAccess.Cassandra.Configurations;
using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Concrete.Cassandra.TableMappers;

public class RemoteOfferProductModelMappers : Mappings
{
    public RemoteOfferProductModelMappers()
    {
        var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        var cassandraConnectionSettings =
            configuration.GetSection("CassandraConnectionSettings").Get<CassandraConnectionSettings>();
        For<RemoteOfferProductModel>()
            .TableName("remote_offer_product_models")
            .KeyspaceName(cassandraConnectionSettings.Keyspace)
            .PartitionKey("id","strategy_id", "status")
            .ClusteringKey(new Tuple<string, SortOrder>("created_at", SortOrder.Descending))
            .Column(u => u.Id, cm => cm.WithName("id").WithDbType(typeof(long)))
            .Column(u => u.StrategyId, cm => cm.WithName("strategy_id").WithDbType(typeof(long)))
            .Column(u => u.Name, cm => cm.WithName("name").WithDbType(typeof(string)))
            .Column(u => u.Count, cm => cm.WithName("count").WithDbType(typeof(float)))
            .Column(u => u.Image, cm => cm.WithName("image").WithDbType(typeof(byte[])))
            .Column(u => u.ImageName, cm => cm.WithName("image_name").WithDbType(typeof(string)))
            .Column(u => u.Status, cm => cm.WithName("status").WithDbType(typeof(bool)))
            .Column(u => u.CreatedAt, cm => cm.WithName("created_at").WithDbType(typeof(DateTimeOffset)));
    }
}