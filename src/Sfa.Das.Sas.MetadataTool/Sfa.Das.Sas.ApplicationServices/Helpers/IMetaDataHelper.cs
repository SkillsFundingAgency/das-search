using System;
using System.Collections.Generic;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.Helpers
{
    public interface IMetaDataHelper
    {
        List<StandardMetaData> GetAllStandardsMetaData();

        StandardMetaData GetStandardMetaData(string id);

        List<FrameworkMetaData> GetAllFrameworksMetaData();

        FrameworkMetaData GetFrameworkMetaData(string id);

        void UpdateFrameworkMetaData(FrameworkMetaData model);
    }
}
