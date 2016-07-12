using System.Collections.Generic;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories
{
    public interface IGenericMetaDataFactory
    {
        T Create<T>(IEnumerable<string> values)
            where T : class;
    }
}
