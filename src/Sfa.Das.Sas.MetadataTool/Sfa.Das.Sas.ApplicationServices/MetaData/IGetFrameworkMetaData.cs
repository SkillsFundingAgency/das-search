using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.MetaData
{
    public interface IGetFrameworkMetaData
    {
        List<FrameworkMetaData> GetFrameworksMetaData();

        FrameworkMetaData GetFrameworkMetaData(int id);
    }
}
