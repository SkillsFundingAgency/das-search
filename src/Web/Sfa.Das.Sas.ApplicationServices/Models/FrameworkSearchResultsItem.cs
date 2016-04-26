namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public sealed class FrameworkSearchResultsItem
    {
        public int FrameworkId { get; set; }

        public string Title { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public int FrameworkCode { get; set; }

        public int PathwayCode { get; set; }

        public int Level { get; set; }
    }
}