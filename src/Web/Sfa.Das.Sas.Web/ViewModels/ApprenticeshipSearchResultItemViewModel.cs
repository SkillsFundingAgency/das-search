namespace Sfa.Das.Sas.Web.ViewModels
{
    public sealed class ApprenticeshipSearchResultItemViewModel
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public string TypicalLengthMessage { get; set; }

        // Frameworks
        public int FrameworkId { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public string Level { get; set; }

        public string ApprenticeshipType { get; set; }
    }
}