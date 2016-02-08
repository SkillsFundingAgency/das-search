using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Models
{
    public class Provider
    {
        public string UkPrn { get; set; }
        public string ProviderName { get; set; }
        public string VenueName { get; set; }
        public string PostCode { get; set; }
        public Coordinate Coordinate { get; set; }
        public int Radius { get; set; }
    }
}
