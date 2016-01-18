using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sfa.Eds.Das.Web.Models
{
    public class StandardDocument
    {
        public int StandardId { get; set; }
        public string Title { get; set; }
        public int NotionalEndLevel { get; set; }
        public string PdfFileName { get; set; }
        public string PdfUrl { get; set; }
        public Attachment File { get; set; }
    }
}