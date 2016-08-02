namespace Sfa.Das.Sas.Indexer.Infrastructure.DapperBD
{
    using System.Collections.Generic;

    public interface IDatabaseProvider
    {
        IEnumerable<T> Query<T>(string query, object param = null);

        T ExecuteScalar<T>(string query);
    }
}