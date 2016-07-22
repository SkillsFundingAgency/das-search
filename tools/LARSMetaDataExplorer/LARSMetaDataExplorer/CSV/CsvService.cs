using System;
using System.Collections.Generic;
using LARSMetaDataExplorer.MetaData;

namespace LARSMetaDataExplorer.CSV
{
    public class CsvService : ICsvService
    {
        private readonly IGenericMetaDataFactory _metaDataFactory;

        public CsvService(IGenericMetaDataFactory metaDataFactory)
        {
            _metaDataFactory = metaDataFactory;
        }

        public ICollection<T> ReadFromString<T>(string csvString)
            where T : class
        {
            var metaDataElements = new List<T>();

            foreach (var line in csvString.Split('\n'))
            {
                var values = line?.Split(new[] { "\",\"" }, StringSplitOptions.None);

                var metaData = _metaDataFactory.Create<T>(values);

                if (metaData != null)
                {
                    metaDataElements.Add(metaData);
                }
            }

            return metaDataElements;
        }
    }
}
