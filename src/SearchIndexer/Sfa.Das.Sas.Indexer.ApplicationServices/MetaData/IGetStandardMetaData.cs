using System.Collections.Generic;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.MetaData
{
    using Sfa.Das.Sas.Indexer.Core.Models;

    public interface IGetStandardMetaData
    {
        List<StandardMetaData> GetStandardsMetaData();
    }
}