using System.Collections.Generic;

namespace LARSMetaDataExplorer.CSV
{
    public interface ICsvService
    {
        ICollection<T> ReadFromString<T>(string csvString)
            where T : class;
    }
}