namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    public interface IVstsService
    {
        IEnumerable<string> GetExistingStandardIds();

        IDictionary<string, string> GetStandards();

        void PushCommit(List<StandardObject> items);
    }
}