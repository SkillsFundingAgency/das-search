namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ProviderSearchViewModel
    {
        public string Title { get; set; }

        public LinkViewModel PreviousPageLink { get; set; }

        public bool HasError { get; set; }

        public string PostUrl { get; set; }

        public int ApprenticeshipId { get; set; }

        public string SearchTerms { get; set; }
    }
}