using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //private readonly ILog _log;

        public MetaDataHelper(IGetStandardMetaData metaDataReader/*, ILog log*/, IGetFrameworkMetaData metaDataFrameworkReader)
        {
            _metaDataReader = metaDataReader;
            //_log = log;
            _metaDataFrameworkReader = metaDataFrameworkReader;
        }

        public List<StandardMetaData> GetAllStandardsMetaData()
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataReader.GetStandardsMetaData());

            // _log.Debug("MetaDataHelper.GetAllStandardsMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.ElaspedMilliseconds });

            return timing.Result.OrderBy(x => x.Id).ToList();
        }

        public List<FrameworkMetaData> GetAllFrameworkMetaData()
        {
            var timing = ExecutionTimer.GetTiming(() => _metaDataFrameworkReader.GetFrameworksMetaData());

            // _log.Debug("MetaDataHelper.GetAllFrameworkMetaData", new TimingLogEntry { ElaspedMilliseconds = timing.ElaspedMilliseconds });

            return timing.Result.OrderBy(x => x.Id).ToList();
        }
    }
}
