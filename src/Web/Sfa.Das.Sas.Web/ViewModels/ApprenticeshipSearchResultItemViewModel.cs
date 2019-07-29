using Sfa.Das.Sas.Web.Models;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public sealed class ApprenticeshipSearchResultItemViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public string Level { get; set; }

        public ApprenticeshipType ApprenticeshipType { get; set; }
    }
}