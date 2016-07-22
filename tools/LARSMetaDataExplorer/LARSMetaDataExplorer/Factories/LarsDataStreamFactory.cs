using System.IO;
using LARSMetaDataToolBox.Services;
using LARSMetaDataToolBox.Settings;
using LARSMetaDataToolBox.Web;
using LARSMetaDataToolBox.Zip;

namespace LARSMetaDataToolBox.Factories
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
