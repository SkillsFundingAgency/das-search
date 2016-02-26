namespace Sfa.Das.ApplicationServices.Models
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Core.Domain.Model;

    public sealed class StandardSearchResultsItem
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public int NotionalEntryLevel { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfUrl { get; set; }

        public string MinimumLength { get; set; }

        public List<string> JobRoles { get; set; }

        public List<string> Keywords { get; set; }

        public TypicalLength TypicalLength { get; set; }
    }
}