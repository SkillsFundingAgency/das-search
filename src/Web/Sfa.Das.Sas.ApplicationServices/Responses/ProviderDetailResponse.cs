
using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.ApplicationServices.Responses
{
    public class ProviderDetailResponse
    {
        public enum ResponseCodes
        {
            Success
        }

        public ResponseCodes StatusCode { get; set; }

        public Provider Provider { get; set; }
    }
}
