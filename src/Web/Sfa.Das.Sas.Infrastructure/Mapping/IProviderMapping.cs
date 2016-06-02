using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public interface IProviderMapping
    {
        ApprenticeshipDetails MapToProvider(StandardProviderSearchResultsItem item);

        ApprenticeshipDetails MapToProvider(FrameworkProviderSearchResultsItem item);
    }
}