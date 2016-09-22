using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Elasticsearch
{
    public interface IProviderLocationSearchProvider
    {
        List<StandardProviderSearchResultsItem> SearchStandardProviders(int standardId, Coordinate coordinates, int page);

        List<FrameworkProviderSearchResultsItem> SearchFrameworkProviders(int id, Coordinate coordinates, int page);
    }
}
