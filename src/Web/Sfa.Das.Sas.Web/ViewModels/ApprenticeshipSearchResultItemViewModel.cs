namespace Sfa.Das.Sas.Web.ViewModels
{
    public sealed class ApprenticeshipSearchResultItemViewModel
    {
        public string StandardId { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        // Frameworks
        public string FrameworkId { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public string Level { get; set; }

        public string ApprenticeshipType { get; set; }
    }
}