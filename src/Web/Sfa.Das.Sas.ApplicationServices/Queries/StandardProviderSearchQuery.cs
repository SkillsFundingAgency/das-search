using MediatR;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public sealed class StandardProviderSearchQuery : ProviderSearchQuery, IAsyncRequest<StandardProviderSearchResponse>
    {
    }
}
