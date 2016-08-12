using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Sfa.Das.Sas.ApplicationServices.Http;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.Services
{

    public class VstsService : IVstsService
    {
        private readonly IAppServiceSettings _appServiceSettings;

        private readonly IJsonMetaDataConvert _jsonMetaDataConvert;

        private readonly IHttpGet _httpHelper;

        //private readonly ILog _logger;

        public VstsService(
            IAppServiceSettings appServiceSettings,
            IJsonMetaDataConvert jsonMetaDataConvert,
            IHttpGet httpHelper/*,
            ILog logger*/)
        {
            _appServiceSettings = appServiceSettings;
            _jsonMetaDataConvert = jsonMetaDataConvert;
            _httpHelper = httpHelper;
            //_logger = logger;
        }

        public IEnumerable<StandardMetaData> GetStandards()
        {
            var standardsDictionary = GetAllFileContents(_appServiceSettings.VstsGitGetFilesUrl);
            return _jsonMetaDataConvert.DeserializeObject<StandardMetaData>(standardsDictionary);
        }

        public StandardMetaData GetStandard(int id)
        {
            var standardsDictionary = GetAllFileContents(_appServiceSettings.VstsGitGetFilesUrl);
            return _jsonMetaDataConvert.DeserializeObject<StandardMetaData>(standardsDictionary).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<VstsFrameworkMetaData> GetFrameworks()
        {
            try
            {
                var frameworksDir = GetAllFileContents(_appServiceSettings.VstsGitGetFrameworkFilesUrl);
                return _jsonMetaDataConvert.DeserializeObject<VstsFrameworkMetaData>(frameworksDir);
            }
            catch (Exception ex)
            {
                //_logger.Error(ex, "Error getting framework meta data");
            }

            return new List<VstsFrameworkMetaData>();
        }

        public VstsFrameworkMetaData GetFramework(int id)
        {
            try
            {
                var frameworksDir = GetAllFileContents(_appServiceSettings.VstsGitGetFrameworkFilesUrl);
                return _jsonMetaDataConvert.DeserializeObject<VstsFrameworkMetaData>(frameworksDir).FirstOrDefault(x => x.Id == id.ToString());
            }
            catch (Exception ex)
            {
                //_logger.Error(ex, "Error getting framework meta data");
            }

            return new VstsFrameworkMetaData();
        }


        private IDictionary<string, string> GetAllFileContents(string vstsBlobUrl)
        {
            var blobs = GetAllBlobs(vstsBlobUrl);

            if (blobs == null)
            {
                return new Dictionary<string, string>(0);
            }

            var apprenticeshipAsJson = new Dictionary<string, string>();

            foreach (var blob in blobs)
            {
                var str = _httpHelper.Get(blob.Url, _appServiceSettings.GitUsername, _appServiceSettings.GitPassword);
                apprenticeshipAsJson.Add(blob.Path, str);
            }

            return apprenticeshipAsJson;
        }

        // Helpers
        private IEnumerable<Entity> GetAllBlobs(string vstsUrl)
        {
            var folderTreeStr = _httpHelper.Get(
                vstsUrl,
                _appServiceSettings.GitUsername,
                _appServiceSettings.GitPassword);
            var tree = JsonConvert.DeserializeObject<GitTree>(folderTreeStr);

            return tree?.Value.Where(x => x.IsBlob);
        }
    }
}
