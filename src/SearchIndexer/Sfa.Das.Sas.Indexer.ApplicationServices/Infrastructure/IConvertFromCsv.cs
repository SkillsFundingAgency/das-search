using System.Collections.Generic;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure
{
    public interface IConvertFromCsv
    {
        ICollection<T> CsvTo<T>(string result)
            where T : class, new();
    }
}