using System.Collections.Generic;

namespace LARSMetaDataToolBox.MetaData
{
    public interface IGenericMetaDataFactory
    {
        T Create<T>(IEnumerable<string> values)
            where T : class;
    }
}
