using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public interface IProviderMapping
    {
        ProviderCourse MapToProvider(StandardProviderSearchResultsItem item);

        ProviderCourse MapToProvider(FrameworkProviderSearchResultsItem item);
    }
}