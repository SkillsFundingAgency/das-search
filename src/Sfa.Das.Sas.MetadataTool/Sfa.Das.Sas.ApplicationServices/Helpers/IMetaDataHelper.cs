using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.Helpers
{
    public interface IMetaDataHelper
    {
        List<StandardMetaData> GetAllStandardsMetaData();

        StandardMetaData GetStandardMetaData(string id);

        List<FrameworkMetaData> GetAllFrameworksMetaData();

        FrameworkMetaData GetFrameworkMetaData(string id);
    }
}
