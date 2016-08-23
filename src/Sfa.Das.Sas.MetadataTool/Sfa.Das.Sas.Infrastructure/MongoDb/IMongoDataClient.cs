using System.Collections.Generic;

namespace Sfa.Das.Sas.Infrastructure.MongoDb
{
    public interface IMongoDataClient
    {
        void Insert<T>(IEnumerable<T> data, string collectionName);

        IEnumerable<T> GetAll<T>(string collectionName);

        T GetById <T>(string collectionName, int id);

        T GetById<T>(string collectionName, string id);
    }
}