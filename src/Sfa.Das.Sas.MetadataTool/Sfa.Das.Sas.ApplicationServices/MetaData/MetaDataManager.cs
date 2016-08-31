using System.Collections.Generic;
using System.Linq;

using Sfa.Das.Sas.ApplicationServices.Services.Interfaces;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.MetaData
{
    public class MetaDataManager : IGetStandardMetaData, IGetFrameworkMetaData, IUpdateMetaData
    {
        private readonly IMetaDataService _metaDataService;

        public MetaDataManager(IMetaDataService metaDataService)
        {
            this._metaDataService = metaDataService;
        }
        
        public List<StandardMetaData> GetStandardsMetaData()
        {
            var standards = _metaDataService.GetStandards()?.ToList();
            
            return standards;
        }

        public StandardMetaData GetStandardMetaData(string id)
        {
            var standard = _metaDataService.GetStandard(id);

            return standard;
        }

        public void GetStandardMetaData(StandardMetaData model)
        {
            _metaDataService.UpdateStandard(model);
        }

        public List<FrameworkMetaData> GetFrameworksMetaData()
        {
            var frameworks = _metaDataService.GetFrameworks()?.ToList();

            return frameworks;
        }

        public FrameworkMetaData GetFrameworkMetaData(string id)
        {
            var framework = _metaDataService.GetFramework(id);

            return framework;
        }

        public void GetFrameworkMetaData(FrameworkMetaData model)
        {
            _metaDataService.UpdateFramework(model);
        }
    }
}