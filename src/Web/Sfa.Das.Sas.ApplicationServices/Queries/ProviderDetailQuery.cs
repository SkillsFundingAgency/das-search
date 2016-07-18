using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    using MediatR;

    public class ProviderDetailQuery : IRequest<DetailProviderResponse>
    {
        public string ProviderId { get; set; }

        public string LocationId { get; set; }

        public string StandardCode { get; set; }

        public string FrameworkId { get; set; }
    }
}