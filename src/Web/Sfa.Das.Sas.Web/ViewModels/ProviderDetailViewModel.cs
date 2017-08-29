using System.Linq;
using SFA.DAS.Apprenticeships.Api.Types.Providers;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ProviderDetailViewModel
   {
       public string TradingNames { get; set; }
        public string Email { get; set; }
        public double EmployerSatisfaction { get; set; }
        public string EmployerSatisfactionMessage { get; set; }
        public bool IsEmployerProvider { get; set; }
        public bool IsHigherEducationInstitute { get; set; }
        public double LearnerSatisfaction { get; set; }
        public string LearnerSatisfactionMessage { get; set; }
        public bool NationalProvider { get; set; }
        public string Phone { get; set; }
        public string ProviderName { get; set; }
        public long UkPrn { get; set; }
        public string Website { get; set; }
    }
}