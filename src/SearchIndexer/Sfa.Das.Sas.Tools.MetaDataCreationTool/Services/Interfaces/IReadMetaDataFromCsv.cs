using System.Collections.Generic;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces
{
    public interface IReadMetaDataFromCsv
    {
        ICollection<T> ReadFromString<T>(string csvString)
            where T : class;
    }
}