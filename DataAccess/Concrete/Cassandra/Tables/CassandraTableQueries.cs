namespace DataAccess.Concrete.Cassandra.Tables
{
    public static class CassandraTableQueries
    {
        public static string RemoteOfferProductModels => "CREATE TABLE IF NOT EXISTS RemoteConfigDatabase.remote_offer_product_models(id bigint, remote_offer_name text, version text, name text, image blob, count decimal, image_name text, status boolean, PRIMARY KEY(id))";
        public static string RemoteOfferModels => "CREATE TABLE IF NOT EXISTS RemoteConfigDatabase.remote_offer_models(id bigint, project_id bigint, name text, is_active boolean, first_price decimal, last_price decimal, version text, player_percent int, is_gift boolean, gift_texture blob, validity_period int, start_time bigint, finish_time bigint, status boolean, PRIMARY KEY(id))";
        public static string InterstitialAdModels => "CREATE TABLE IF NOT EXISTS RemoteConfigDatabase.interstitial_ad_models(id bigint, name text,project_id bigint, version text, player_percent int, is_adv_settings_active boolean, status boolean, PRIMARY KEY(id))";
        public static string AdvStrategies => "CREATE TABLE IF NOT EXISTS RemoteConfigDatabase.adv_strategies(id bigint, name text, count decimal, strategy_version text, strategy_name text, status boolean, PRIMARY KEY(id))";
        
    }
}





