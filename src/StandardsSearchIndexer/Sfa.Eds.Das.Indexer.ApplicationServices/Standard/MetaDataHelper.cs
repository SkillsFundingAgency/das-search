namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Core.Services;
    using Newtonsoft.Json;

    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.Core.Models;
    using Sfa.Eds.Das.Indexer.Core.Models.Framework;

    public class MetaDataHelper : IMetaDataHelper
    {
        private readonly IGetStandardMetaData _metaDataReader;

        private readonly IGenerateStandardMetaData _metaDataWriter;

        private readonly IGetFrameworkMetaData _metaDataFrameworkReader;

        private readonly ILog _log;

        public MetaDataHelper(IGetStandardMetaData metaDataReader, IGenerateStandardMetaData metaDataGenerator, ILog log, IGetFrameworkMetaData metaDataFrameworkReader)
        {
            _metaDataReader = metaDataReader;
            _metaDataWriter = metaDataGenerator;
            _log = log;
            _metaDataFrameworkReader = metaDataFrameworkReader;
        }

        public List<StandardMetaData> GetAllStandardsMetaData()
        {
            var stopwatch = new Stopwatch();
            var standardsMetaDataJson = _metaDataReader.GetAllAsJson();
            stopwatch.Stop();
            _log.Debug("MetaDataHelper.GetAllStandardsMetaData", new Dictionary<string, object> { { "ExecutionTime", stopwatch.ElapsedMilliseconds } });

            var standardsMetaData = new List<StandardMetaData>();

            foreach (var item in standardsMetaDataJson)
            {
                try
                {
                    standardsMetaData.Add(JsonConvert.DeserializeObject<StandardMetaData>(item.Value));
                }
                catch (JsonReaderException ex)
                {
                    _log.Warn($"Couldn't deserialise meta data for: {item.Key}", ex);
                }
            }

            return standardsMetaData;
        }

        public void UpdateMetadataRepository()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            _metaDataWriter.GenerateStandardMetadataFiles();
            stopwatch.Stop();
            _log.Debug("MetaDataHelper.UpdateMetadataRepository", new Dictionary<string, object> { { "ExecutionTime", stopwatch.ElapsedMilliseconds } });
        }

        public List<FrameworkMetaData> GetAllFrameworkMetaData()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var frameworks = _metaDataFrameworkReader.GetAllFrameworks();
            stopwatch.Stop();
            _log.Debug("MetaDataHelper.GetAllFrameworkMetaData", new Dictionary<string, object> { { "ExecutionTime", stopwatch.ElapsedMilliseconds } });

            return frameworks;
        }
    }
}