using System;

namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    public sealed class Standard
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int NotionalEndLevel { get; set; }

        public string PdfFileName { get; set; }

        public Uri Pdf { get; set; }
    }
}
