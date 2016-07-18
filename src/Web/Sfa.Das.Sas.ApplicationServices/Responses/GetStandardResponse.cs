using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.ApplicationServices.Responses
{
    public class GetStandardResponse
    {
        public enum ResponseCodes
        {
            Success,
            InvalidStandardId,
            StandardNotFound
        }

        public ResponseCodes StatusCode { get; set; }

        public Standard Standard { get; set; }

        public bool IsShortlisted { get; set; }

        public string SearchTerms { get; set; }
    }
}
