using MediatR;
using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public sealed class StandardProviderSearchQuery : ProviderSearchQuery, IAsyncRequest<StandardProviderSearchResponse>, IRequest<GetStandardProvidersResponse>
    {
    }
}
