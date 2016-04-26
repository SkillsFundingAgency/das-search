using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public interface IProviderMapping
    {
        Provider MapToProvider(StandardProviderSearchResultsItem item);

        Provider MapToProvider(FrameworkProviderSearchResultsItem item);
    }
}