using System;
using System.Collections.Generic;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories.MetaData
{
    public interface IMetaDataFactory
    {
        Type MetaDataType { get; }
        object Create(IReadOnlyList<string> values);
    }
}
