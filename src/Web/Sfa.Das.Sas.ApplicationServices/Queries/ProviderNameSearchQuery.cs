using MediatR;
using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public sealed class ProviderNameSearchQuery : IAsyncRequest<ProviderNameSearchResponse>
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; }
    }
}
