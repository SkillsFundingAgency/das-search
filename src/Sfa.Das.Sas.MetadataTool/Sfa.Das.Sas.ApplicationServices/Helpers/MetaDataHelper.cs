using System.Collections.Generic;
using System.Linq;

using Sfa.Das.Sas.ApplicationServices.MetaData;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Core.Logging.Metrics;
using Sfa.Das.Sas.Core.Logging.Models;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.Helpers
{
    public class MetaDataHelper : IMetaDataHelper
    {
        private readonly IGetStandardMetaData _metaDataReader;


        private readonly IGetFrameworkMetaData _metaDataFrameworkReader;

        private readonly ILog _log;

        public MetaDataHelper(IGetStandardMetaData metaDataReader, ILog log, IGetFrameworkMetaData metaDataFrameworkReader)
        {
            _metaDataReader = metaDataReader;
            _log = log;
            _metaDataFrameworkReader = metaDataFrameworkReader;
        }

        public List<StandardMetaData> GetAllStandardsMetaData()
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataReader.GetStandardsMetaData());

            _log.Debug("MetaDataHelper.GetAllStandardsMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.ElaspedMilliseconds });

            return timing.Result.OrderBy(x => x.Id).ToList();
        }

        public StandardMetaData GetStandardMetaData(int id)
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataReader.GetStandardMetaData(id));

            _log.Debug("MetaDataHelper.GetStandardMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.ElaspedMilliseconds });

            return timing.Result;
        }

        public List<FrameworkMetaData> GetAllFrameworksMetaData()
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataFrameworkReader.GetFrameworksMetaData());

            _log.Debug("MetaDataHelper.GetAllFrameworksMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.ElaspedMilliseconds });

            return timing.Result.OrderBy(x => x.Id).ToList();
        }

        public FrameworkMetaData GetFrameworkMetaData(int id)
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataFrameworkReader.GetFrameworkMetaData(id));

            _log.Debug("MetaDataHelper.GetFrameworkMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.ElaspedMilliseconds });

            return timing.Result;
        }
    }
}
