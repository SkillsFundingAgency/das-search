using System.Collections.Generic;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.MetaData
{
    public interface IGetStandardMetaData
    {
        List<StandardMetaData> GetStandardsMetaData();

        StandardMetaData GetStandardMetaData(string id);
    }
}
