namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using MongoDB.Driver;

    public interface IMongoSettings
    {
        string DatabaseName { get; }

        string CollectionNameStandards { get; }

        string CollectionNameFrameworks { get; }

        string ConnectionString { get; }
    }
}