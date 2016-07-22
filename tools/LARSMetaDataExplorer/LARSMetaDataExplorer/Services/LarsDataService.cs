using System;
using System.IO;
using System.Linq;
using LARSMetaDataToolBox.Settings;
using LARSMetaDataToolBox.Web;
using LARSMetaDataToolBox.Zip;

namespace LARSMetaDataToolBox.Services
{
    public class LarsDataService
    {
        private readonly IHttpClient _httpClient;
        private readonly IAngleSharpService _angleSharpService;
        private readonly IUnzipStream _zipFileExtractor;
        private readonly IAppSettings _appSettings;

        public LarsDataService(
            IHttpClient httpClient,
            IAngleSharpService angleSharpService,
            IUnzipStream zipFileExtractor,
            IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _angleSharpService = angleSharpService;
            _zipFileExtractor = zipFileExtractor;
            _appSettings = appSettings;
        }

        public Stream GetLarsCsvStream()
        {
            var zipFilePath = GetZipFilePath();

            return _httpClient.GetFile(zipFilePath);
        }

        private string GetZipFilePath()
        {
            var url = $"{_appSettings.ImServiceBaseUrl}/{_appSettings.ImServiceUrl}";

            var link = _angleSharpService.GetLinks(url, "li a", "LARS CSV");
            var linkEndpoint = link.FirstOrDefault();
            var fullLink = linkEndpoint != null ? $"{_appSettings.ImServiceBaseUrl}/{linkEndpoint}" : string.Empty;

            if (string.IsNullOrEmpty(fullLink))
            {
                throw new Exception($"Can not find LARS zip file. Url: {url}");
            }

            return fullLink;
        }
    }
}
