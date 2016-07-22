using System.IO;
using LARSMetaDataExplorer.Services;
using LARSMetaDataExplorer.Settings;
using LARSMetaDataExplorer.Web;
using LARSMetaDataExplorer.Zip;

namespace LARSMetaDataExplorer.Factories
{
    public class LarsDataStreamFactory
    {
        private readonly LarsDataService _larsDataService;

        public LarsDataStreamFactory()
        {
            var httpClient = new HttpClient();

            _larsDataService = new LarsDataService(
                httpClient, 
                new AngleSharpService(httpClient), 
                new ZipFileExtractor(), 
                new AppSettings());
        }

        public Stream GetDataStream()
        {
            return _larsDataService.GetLarsCsvStream();
        }
    }
}
