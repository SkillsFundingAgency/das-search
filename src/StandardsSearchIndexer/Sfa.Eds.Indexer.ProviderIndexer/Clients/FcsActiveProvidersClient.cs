namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class FcsActiveProvidersClient : IActiveProviderClient
    {
        private readonly IVstsService _vstsService;

        public FcsActiveProvidersClient(IVstsService vstsService)
        {
            _vstsService = vstsService;
        }

        public Task<IEnumerable<string>> GetProviders()
        {
            throw new NotImplementedException();
        }
    }
}