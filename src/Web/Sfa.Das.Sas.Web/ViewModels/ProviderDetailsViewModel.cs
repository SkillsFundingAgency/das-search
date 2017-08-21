using System.Linq;
using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ProviderDetailsViewModel
   {
        //    public string ContentDisclaimer { get; } = @"Skills Funding Agency cannot guarantee the accuracy of course information on this site and makes
        //           no representations about the quality of any courses which appear on the site. Skills Funding Agency
        //           is not liable for any losses suffered as a result of any party relying on the course information
        //       provided.";

       public string TradingNames { get; set; }

        public string Email { get; set; }
        public double EmployerSatisfaction { get; set; }
        public bool IsEmployerProvider { get; set; }
        public bool IsHigherEducationInstitute { get; set; }
        public double LearnerSatisfaction { get; set; }
        public bool NationalProvider { get; set; }
        public string Phone { get; set; }
        public string ProviderName { get; set; }
        public long Ukprn { get; set; }
        public string Uri { get; set; }
        public string Website { get; set; }
    }
}