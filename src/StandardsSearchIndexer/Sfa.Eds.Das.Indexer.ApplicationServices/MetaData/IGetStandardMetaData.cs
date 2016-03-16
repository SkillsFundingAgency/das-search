namespace Sfa.Eds.Das.Indexer.ApplicationServices.MetaData
{
    using System.Collections.Generic;

    public interface IGetStandardMetaData
    {
        IDictionary<string, string> GetAllAsJson();
    }
}