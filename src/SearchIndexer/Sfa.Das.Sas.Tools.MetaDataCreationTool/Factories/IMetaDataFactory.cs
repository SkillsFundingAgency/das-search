using System.Collections.Generic;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories
{
    public interface IMetaDataFactory
    {
        T Create<T>(IEnumerable<string> values)
            where T : class;
    }
}
