namespace DataAccess.Concrete.Cassandra.Tables
{
    public static class CassandraTableQueries
    {
        public static string RemoteOfferEventModels => "CREATE TABLE IF NOT EXISTS user_projects(id bigint, status boolean, PRIMARY KEY(id))";
        public static string InterstitialAdEventModels => "interstitialAdEventModels";
        public static string InterstitialAdHistoryModels => "interstitialAdHistoryModels";
        public static string RemoteOfferHistoryModels => "remoteOfferHistoryModels";
        public static string RemoteOfferModels => "remoteOfferModels";
        public static string InterstitialAdModels => "interstitialAdModels";
    }
}

