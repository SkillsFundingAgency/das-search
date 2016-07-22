using System;
using System.Collections.Generic;

namespace LARSMetaDataToolBox.MetaData.MetaDataFactories
{
    public interface IMetaDataFactory
    {
        Type MetaDataType { get; }
        object Create(IReadOnlyList<string> values);
    }
}
