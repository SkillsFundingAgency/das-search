using System.Collections.Generic;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.MetaData
{
    public interface IGetFrameworkMetaData
    {
        List<FrameworkMetaData> GetAllFrameworks();
    }
}
