using SFA.DAS.Apprenticeships.Api.Types;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ProviderSearchViewModel
    {
        public string Title { get; set; }

        public bool HasError { get; set; }

        public string PostUrl { get; set; }

        public string ApprenticeshipId { get; set; }

        public ApprenticeshipTrainingType ApprenticeshipType { get; set; }

        public string PostCode { get; set; }

        public string SearchTerms { get; set; }

        public bool? IsLevyPayingEmployer { get; set; }

        public string ErrorMessage { get; set; }
    }
}