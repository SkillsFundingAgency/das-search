using Sfa.Das.ApplicationServices.Models;
using Sfa.Eds.Das.Core.Domain.Model;

namespace Sfa.Eds.Das.Infrastructure.Mapping
{
    public interface IProviderMapping
    {
        Provider MapToProvider(StandardProviderSearchResultsItem item);

        Provider MapToProvider(FrameworkProviderSearchResultsItem item);
    }
}