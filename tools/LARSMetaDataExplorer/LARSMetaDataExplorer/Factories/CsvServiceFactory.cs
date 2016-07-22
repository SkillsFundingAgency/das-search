using System.Collections.Generic;
using LARSMetaDataExplorer.CSV;
using LARSMetaDataExplorer.MetaData;
using LARSMetaDataExplorer.MetaData.MetaDataFactories;

namespace LARSMetaDataExplorer.Factories
{
    public class CsvServiceFactory
    {
        private readonly LarsMetaDataFactory _metaDataFactory;

        public CsvServiceFactory()
        {
            _metaDataFactory = new LarsMetaDataFactory(new List<IMetaDataFactory>
            {
                new FrameworkMetaDataFactory(),
                new FrameworkAimMetaDataFactory(),
                new FrameworkComponentTypeMetaDataFactory(),
                new LearningDeliveryMetaDataFactory(),
                new StandardMetaDataFactory(),
                new FundingMetaDataFactory()
               });
        }

        public ICsvService Create()
        {
            return new CsvService(_metaDataFactory);
        }
    }
}
