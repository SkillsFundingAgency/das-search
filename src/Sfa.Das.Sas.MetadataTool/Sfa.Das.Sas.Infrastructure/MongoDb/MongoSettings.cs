namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using System.Configuration;

    public class MongoSettings : IMongoSettings
    {
        public string DatabaseName => ConfigurationManager.AppSettings["Mongo.DatabaseName"];

        public string CollectionNameStandards => ConfigurationManager.AppSettings["Mongo.CollectionNameStandards"];

        public string CollectionNameFrameworks => ConfigurationManager.AppSettings["Mongo.CollectionNameFrameworks"];

        public string ConnectionString => ConfigurationManager.AppSettings["Mongo.ConnectionString"];
    }
}
