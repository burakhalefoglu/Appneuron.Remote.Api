﻿using Cassandra.Mapping;
using Core.DataAccess.Cassandra.Configurations;
using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Concrete.Cassandra.TableMappers;

public class RemoteOfferModelMappers : Mappings
{
    public RemoteOfferModelMappers()
    {
        var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
        var cassandraConnectionSettings =
            configuration.GetSection("CassandraConnectionSettings").Get<CassandraConnectionSettings>();
        For<RemoteOfferModel>()
            .TableName("remote_offer_models")
            .KeyspaceName(cassandraConnectionSettings.Keyspace)
            .PartitionKey("name", "project_id", "version", "status")
            .ClusteringKey(new Tuple<string, SortOrder>("created_at", SortOrder.Descending))
            .Column(u => u.Id, cm => cm.WithName("id").WithDbType(typeof(long)))
            .Column(u => u.ProjectId, cm => cm.WithName("project_id").WithDbType(typeof(long)))
            .Column(u => u.Name, cm => cm.WithName("name").WithDbType(typeof(string)))
            .Column(u => u.Version, cm => cm.WithName("version").WithDbType(typeof(string)))
            .Column(u => u.PlayerPercent, cm => cm.WithName("player_percent").WithDbType(typeof(int)))
            .Column(u => u.FirstPrice, cm => cm.WithName("first_price").WithDbType(typeof(float)))
            .Column(u => u.LastPrice, cm => cm.WithName("last_price").WithDbType(typeof(float)))
            .Column(u => u.IsGift, cm => cm.WithName("is_gift").WithDbType(typeof(bool)))
            .Column(u => u.GiftTexture, cm => cm.WithName("gift_texture").WithDbType(typeof(byte[])))
            .Column(u => u.ValidityPeriod, cm => cm.WithName("validity_period").WithDbType(typeof(int)))
            .Column(u => u.StartTime, cm => cm.WithName("start_time").WithDbType(typeof(long)))
            .Column(u => u.FinishTime, cm => cm.WithName("finish_time").WithDbType(typeof(long)))
            .Column(u => u.Status, cm => cm.WithName("status").WithDbType(typeof(bool)))
            .Column(u => u.Terminated, cm => cm.WithName("terminated").WithDbType(typeof(bool)))
            .Column(u => u.CreatedAt, cm => cm.WithName("created_at").WithDbType(typeof(DateTimeOffset)));
    }
}