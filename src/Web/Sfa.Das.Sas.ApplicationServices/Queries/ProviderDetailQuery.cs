using MediatR;
using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public sealed class ProviderDetailQuery : IAsyncRequest<ProviderDetailResponse>
    {
        public long Prn { get; set; }
    }
}
