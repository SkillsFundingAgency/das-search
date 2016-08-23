namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using System.Collections.Generic;

    using MongoDB.Driver;

    public class MongoDataClient : IMongoDataClient
    {
        private readonly IMongoDatabase _database;

        public MongoDataClient(IMongoSettings mongoSettings)
        {
            var client = new MongoClient(mongoSettings.ConnectionString);
            _database = client.GetDatabase(mongoSettings.DatabaseName);
        }

        public void Insert<T>(IEnumerable<T> data, string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            collection.InsertMany(data);
        }

        public IEnumerable<T> GetAll<T>(string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var all = collection.FindSync<T>(FilterDefinition<T>.Empty);

            return all.ToList();
        }

        public T GetById<T>(string collectionName, int id)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.AnyEq("_id", id);
            var record = collection.Find(filter);

            return record.SingleOrDefault();
        }

        public T GetById<T>(string collectionName, string id)
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.AnyEq("_id", id);
            var record = collection.Find(filter);

            return record.SingleOrDefault();
        }
    }
}
