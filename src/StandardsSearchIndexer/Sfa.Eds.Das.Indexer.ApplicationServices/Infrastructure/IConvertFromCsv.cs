namespace Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure
{
    using System.Collections.Generic;

    public interface IConvertFromCsv
    {
        ICollection<T> CsvTo<T>(string result) where T : class, new();
    }
}