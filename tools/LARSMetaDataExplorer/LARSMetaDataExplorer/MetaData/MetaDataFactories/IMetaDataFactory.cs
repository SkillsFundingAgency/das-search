using System;
using System.Collections.Generic;

namespace LARSMetaDataExplorer.MetaData.MetaDataFactories
{
    public interface IMetaDataFactory
    {
        Type MetaDataType { get; }
        object Create(IReadOnlyList<string> values);
    }
}
