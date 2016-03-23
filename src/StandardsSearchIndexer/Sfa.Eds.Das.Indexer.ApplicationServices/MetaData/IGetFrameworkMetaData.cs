namespace Sfa.Eds.Das.Indexer.ApplicationServices.MetaData
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    public interface IGetFrameworkMetaData
    {
        List<FrameworkMetaData> GetAllFrameworks();
    }
}
