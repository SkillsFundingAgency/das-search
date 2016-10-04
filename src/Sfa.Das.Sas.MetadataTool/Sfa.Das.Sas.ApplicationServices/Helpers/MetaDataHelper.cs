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

        private readonly IUpdateMetaData _metaDataUpdate;

        public MetaDataHelper(IGetStandardMetaData metaDataReader, ILog log, IGetFrameworkMetaData metaDataFrameworkReader, IUpdateMetaData metaDataUpdate)
        {
            _metaDataReader = metaDataReader;
            _log = log;
            _metaDataFrameworkReader = metaDataFrameworkReader;
            _metaDataUpdate = metaDataUpdate;
        }

        public List<StandardMetaData> GetAllStandardsMetaData()
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataReader.GetStandardsMetaData());

            _log.Debug("MetaDataHelper.GetAllStandardsMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.ElaspedMilliseconds });

            return timing.Result.OrderBy(x => x.ApprenticeshipId).ToList();
        }

        public StandardMetaData GetStandardMetaData(string id)
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

        public FrameworkMetaData GetFrameworkMetaData(string id)
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataFrameworkReader.GetFrameworkMetaData(id));

            _log.Debug("MetaDataHelper.GetFrameworkMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.ElaspedMilliseconds });

            return timing.Result;
        }

        public void UpdateFrameworkMetaData(FrameworkMetaData model)
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataUpdate.GetFrameworkMetaData(model));

            _log.Debug("MetaDataHelper.GetFrameworkMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.TotalMilliseconds });
        }

        public void UpdateStandardMetaData(StandardMetaData model)
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataUpdate.GetStandardMetaData(model));

            _log.Debug("MetaDataHelper.GetStandardMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.TotalMilliseconds });
        }
    }
}
