using System.Collections.Generic;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.MetaData
{
    public interface IGetFrameworkMetaData
    {
        List<FrameworkMetaData> GetFrameworksMetaData();

        FrameworkMetaData GetFrameworkMetaData(string id);
    }
}
