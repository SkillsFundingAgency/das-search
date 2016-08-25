namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using System;
    using System.Collections.Generic;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using NLog.LayoutRenderers.Wrappers;

    using Sfa.Das.Sas.Infrastructure.MongoDb.Models;

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
            var filter = Builders<T>.Filter.AnyEq("_id", Guid.Parse(id));
            var record = collection.Find(filter);

            return record.SingleOrDefault();
        }

        public void Save<T>(T model, string collectionName)
            where T : IMongoDataType
        {
            var collection = _database.GetCollection<T>(collectionName);
            collection.UpdateOne(
                Builders<T>.Filter.Eq(x => x.Id, model.Id),
                Builders<T>.Update.Set(x => x, model));
        }

        public void Save<T>(T model, FilterDefinition<T> filter, UpdateDefinition<T> update, string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            collection.UpdateOne(filter, update);
        }
    }
}
