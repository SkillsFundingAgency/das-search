using System.Collections.Generic;
using Newtonsoft.Json;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Models.Git;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    public class MetadataApiService : IMetadataApiService
    {
        private readonly IHttpGet _httpService;
        private readonly IAppServiceSettings _appServiceSettings;

        public MetadataApiService(IHttpGet httpService, IAppServiceSettings appServiceSettings)
        {
            _httpService = httpService;
            _appServiceSettings = appServiceSettings;
        }

        public List<StandardMetaData> GetStandards()
        {
            var response = _httpService.Get(CreateApprenticeshipUri("Standard"), string.Empty, string.Empty);
            var standards = ManageApprenticeshipResponse<List<StandardMetaData>>(response);

            return standards;
        }

        public List<VstsFrameworkMetaData> GetFrameworks()
        {
            var response = _httpService.Get(CreateApprenticeshipUri("Framework"), string.Empty, string.Empty);
            var repositoryFrameworks = ManageApprenticeshipResponse<List<VstsFrameworkMetaData>>(response);

            return repositoryFrameworks;
        }

        private T ManageApprenticeshipResponse<T>(string response)
            where T : new()
        {
            return response != string.Empty ? JsonConvert.DeserializeObject<T>(response) : new T();
        }

        private string CreateApprenticeshipUri(string apprenticeship)
        {
            return string.Concat(_appServiceSettings.MetadataApiUri, apprenticeship);
        }
    }
}
