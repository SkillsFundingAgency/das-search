using System.Collections.Generic;
using System.Linq;

using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.MetaData
{
    public class MetaDataManager : IGetStandardMetaData, IGetFrameworkMetaData
    {

        private readonly IMappingService _mappingService;

        private readonly IVstsService _vstsService;

        public MetaDataManager(
            IVstsService vstsService,
            IMappingService mappingService)
        {
            _vstsService = vstsService;
            _mappingService = mappingService;
        }
        
        public List<StandardMetaData> GetStandardsMetaData()
        {
            var standards = _vstsService.GetStandards()?.ToList();
            
            return standards;
        }

        public StandardMetaData GetStandardMetaData(int id)
        {
            var standard = _vstsService.GetStandard(id);

            return standard;
        }

        public List<FrameworkMetaData> GetFrameworksMetaData()
        {
            var frameworks = _vstsService.GetFrameworks()?.ToList();

            return _mappingService.Map<List<VstsFrameworkMetaData>, List<FrameworkMetaData>>(frameworks);
        }

        public FrameworkMetaData GetFrameworkMetaData(int id)
        {
            var framework = _vstsService.GetFramework(id);

            return _mappingService.Map<VstsFrameworkMetaData, FrameworkMetaData>(framework);
        }
    }
}
