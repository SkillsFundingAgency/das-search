namespace Sfa.Eds.Das.ProviderIndexer.Clients
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Eds.Das.Indexer.Common.Extensions;
    using Sfa.Eds.Das.ProviderIndexer.Models;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class FcsActiveProvidersClient : IActiveProviderClient
    {
        private readonly ISettings _settings;

        private readonly IVstsClient _vstsClient;

        public FcsActiveProvidersClient(IVstsClient vstsClient, ISettings settings)
        {
            _vstsClient = vstsClient;
            _settings = settings;
        }

        public IEnumerable<string> GetProviders()
        {
            Func<string> func = LoadProvidersFromVsts;
            var result = func.RetryWebRequest();
            var records = result.FromCsv<FcsProviderRecord>();
            return records.Select(x => x.UkPrn);
        }

        private string LoadProvidersFromVsts()
        {
            return _vstsClient.GetFileContent($"fcs/{_settings.EnvironmentName}/fcs-active.csv");
        }
    }
}