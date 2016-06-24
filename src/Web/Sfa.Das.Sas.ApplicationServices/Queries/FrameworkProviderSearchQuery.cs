using MediatR;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public sealed class FrameworkProviderSearchQuery : ProviderSearchQuery, IAsyncRequest<FrameworkProviderSearchResponse>
    {
    }
}
