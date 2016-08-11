using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Extensions;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.MetaData
{
    public class MetaDataManager : IGetStandardMetaData, IGetFrameworkMetaData
    {
        // private readonly ILog _logger;

        private readonly IMappingService _mappingService;

        private readonly IVstsService _vstsService;

        public MetaDataManager(
            IVstsService vstsService,
            IMappingService mappingService/*,
            ILog logger*/)
        {
            _vstsService = vstsService;
            //_logger = logger;
            _mappingService = mappingService;
        }
        
        public List<StandardMetaData> GetStandardsMetaData()
        {
            var standards = _vstsService.GetStandards()?.ToList();
            
            return standards;
        }

        public List<FrameworkMetaData> GetFrameworksMetaData()
        {
            var frameworks = _vstsService.GetFrameworks()?.ToList();

            return _mappingService.Map<List<VstsFrameworkMetaData>, List<FrameworkMetaData>>(frameworks);
        }
    }
}
