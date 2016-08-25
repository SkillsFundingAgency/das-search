using System.Collections.Generic;

namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    using MongoDB.Driver;

    using Sfa.Das.Sas.Infrastructure.MongoDb.Models;

    public interface IMongoDataClient
    {
        void Insert<T>(IEnumerable<T> data, string collectionName);

        IEnumerable<T> GetAll<T>(string collectionName);

        T GetById <T>(string collectionName, int id);

        T GetById<T>(string collectionName, string id);

        void Save<T>(T model, FilterDefinition<T> filter, UpdateDefinition<T> update, string collectionName);

        void Save<T>(T model, string collectionName) where T : IMongoDataType;
    }
}