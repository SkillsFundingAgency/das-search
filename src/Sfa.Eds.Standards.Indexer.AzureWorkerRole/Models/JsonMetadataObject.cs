using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models
{
    public class JsonMetadataObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int NotionalEndLevel { get; set; }
        public string PdfFileName { get; set; }
        public string Pdf { get; set; }
    }
}
