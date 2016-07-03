using System;
using System.Collections.Generic;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Factories;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    public class CsvService : IReadMetaDataFromCsv
    {
        private readonly IMetaDataFactory _metaDataFactory;

        public CsvService(IMetaDataFactory metaDataFactory)
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
