using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    using MediatR;

    public class ProviderDetailQuery : IRequest<DetailProviderResponse>
    {
        public int Ukprn { get; set; }

        public int LocationId { get; set; }

        public string StandardCode { get; set; }

        public string FrameworkId { get; set; }

        public bool IsLevyPayingEmployer { get; set; }
    }
}