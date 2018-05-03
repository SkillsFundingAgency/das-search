using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Core.Domain.Model
{
    public class ProviderSearchItem
    {
        public int UkPrn { get; set; }
        public string ProviderName { get; set; }
        public List<string> Aliases { get; set; }
    }
}
