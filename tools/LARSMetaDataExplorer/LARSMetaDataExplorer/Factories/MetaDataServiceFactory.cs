using LARSMetaDataToolBox.MetaData;
using LARSMetaDataToolBox.Services;

namespace LARSMetaDataToolBox.Factories
{
    public class MetaDataServiceFactory
    {
        private readonly LarsDataStreamFactory _larsDataStreamFactory;
        private readonly CsvServiceFactory _csvServiceFactory;

        public MetaDataServiceFactory(LarsDataStreamFactory larsDataStreamFactory, CsvServiceFactory csvServiceFactory)
        {
            _larsDataStreamFactory = larsDataStreamFactory;
            _csvServiceFactory = csvServiceFactory;
        }

        public MetaDataService CreateService()
        {
            return new MetaDataService(_larsDataStreamFactory.GetDataStream(), _csvServiceFactory.Create());
        }
    }
}
