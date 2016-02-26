namespace Sfa.Eds.Das.Core.Domain.Model
{
    using System.Collections.Generic;

    public sealed class Standard
    {
        public int StandardId { get; set; }

        public string Title { get; set; }

        public int NotionalEntryLevel { get; set; }

        public int NotionalEndLevel { get; set; }

        public List<string> JobRoles { get; set; }

        public List<string> Keywords { get; set; }

        public TypicalLength TypicalLength { get; set; }

        public string PdfUrl { get; set; }

        public string MinimumLength { get; set; }
    }
}
