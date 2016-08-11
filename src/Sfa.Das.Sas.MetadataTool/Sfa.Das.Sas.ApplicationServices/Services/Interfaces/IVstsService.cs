using System.Collections.Generic;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Models;

namespace Sfa.Das.Sas.ApplicationServices.Services
{
    public interface IVstsService
    {
        IEnumerable<StandardMetaData> GetStandards();

        IEnumerable<VstsFrameworkMetaData> GetFrameworks();
    }
}