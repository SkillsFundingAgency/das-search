using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Services;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Provider
{
    public class FcsActiveProvidersClient : IGetActiveProviders
    {
        private readonly IAppServiceSettings _appServiceSettings;

        private readonly IConvertFromCsv _convertFromCsv;

        private readonly IVstsClient _vstsClient;

        public FcsActiveProvidersClient(IVstsClient vstsClient, IAppServiceSettings appServiceSettings, IConvertFromCsv convertFromCsv)
        {
            _vstsClient = vstsClient;
            _appServiceSettings = appServiceSettings;
            _convertFromCsv = convertFromCsv;
        }

        public IEnumerable<int> GetActiveProviders()
        {
            var records = _convertFromCsv.CsvTo<ActiveProviderCsvRecord>(LoadProvidersFromVsts());
            return records.Select(x => x.UkPrn);
        }

        private string LoadProvidersFromVsts()
        {
            return _vstsClient.GetFileContent($"fcs/{_appServiceSettings.EnvironmentName}/fcs-active.csv");
        }
    }
}