namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class FcsActiveProvidersClient : IActiveProviderClient
    {
        private readonly IVstsClient _vstsClient;

        public FcsActiveProvidersClient(IVstsClient vstsClient)
        {
            _vstsClient = vstsClient;
        }

        public async Task<IEnumerable<string>> GetProviders()
        {
            return _vstsClient.GetAllFileContents("fcs");
        }
    }
}