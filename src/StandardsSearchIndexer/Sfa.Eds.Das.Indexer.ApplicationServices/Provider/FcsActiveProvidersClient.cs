namespace Sfa.Eds.Das.Indexer.ApplicationServices.Provider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure;
    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Core;

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
            Func<string> func = LoadProvidersFromVsts;
            //var result = func.RetryWebRequest();
            var result = func.Invoke();
            var records = _convertFromCsv.CsvTo<ActiveProviderCsvRecord>(result);
            return records.Select(x => x.UkPrn);
        }

        private string LoadProvidersFromVsts()
        {
            return _vstsClient.GetFileContent($"fcs/{_appServiceSettings.EnvironmentName}/fcs-active.csv");
        }
    }
}