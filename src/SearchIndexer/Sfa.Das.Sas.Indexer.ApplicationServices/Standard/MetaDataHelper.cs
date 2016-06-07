using System.Collections.Generic;
using System.Diagnostics;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Indexer.Core.Models;
using Sfa.Das.Sas.Indexer.Core.Models.Framework;
using Sfa.Das.Sas.Indexer.Core.Services;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Standard
{
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
            var standardsMetaData = _metaDataReader.GetStandardsMetaData();
            stopwatch.Stop();
            _log.Debug("MetaDataHelper.GetAllStandardsMetaData", new Dictionary<string, object> { { "ExecutionTime", stopwatch.ElapsedMilliseconds } });

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