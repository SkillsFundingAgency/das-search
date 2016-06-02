using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Core.Domain.Model
{
    public class Provider
    {
        public int Id { get; set; }

        public string UkPrn { get; set; }

        public string Name { get; set; }

        public ContactInformation ContactInformation { get; set; }

    }
}
