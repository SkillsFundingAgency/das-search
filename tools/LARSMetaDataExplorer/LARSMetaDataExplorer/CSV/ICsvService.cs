using System.Collections.Generic;

namespace LARSMetaDataToolBox.CSV
{
    public interface ICsvService
    {
        ICollection<T> ReadFromString<T>(string csvString)
            where T : class;
    }
}