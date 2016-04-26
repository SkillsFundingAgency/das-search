using System.Collections.Generic;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.MetaData
{
    public interface IGetStandardMetaData
    {
        IDictionary<string, string> GetAllAsJson();
    }
}