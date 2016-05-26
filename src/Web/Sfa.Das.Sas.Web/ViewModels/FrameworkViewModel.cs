namespace Sfa.Das.Sas.Web.ViewModels
{
    public class FrameworkViewModel
    {
        public int FrameworkId { get; set; }

        public string Title { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public int FrameworkCode { get; set; }

        public int PathwayCode { get; set; }

        public int Level { get; set; }

        // Page specific

        public LinkViewModel SearchResultLink { get; set; }
    }
}