using System.Collections.Generic;

namespace LARSMetaDataExplorer.MetaData
{
    public interface IGenericMetaDataFactory
    {
        T Create<T>(IEnumerable<string> values)
            where T : class;
    }
}
