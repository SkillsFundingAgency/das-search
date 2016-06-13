using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Indexer.Core.Logging.Models
{
    public class ElasticSearchLogEntry : ILogEntry
    {
        public string Name => "ElasticSearch";

        public int? ReturnCode { get; set; }

        public long? SearchTime { get; set; }

        public double NetworkTime { get; set; }

        public string Url { get; set; }

        public string Body { get; set; }
    }
}
