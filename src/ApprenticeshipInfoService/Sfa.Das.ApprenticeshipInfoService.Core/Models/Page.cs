using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    public class Page
    {
        public int Next { get; set; }

        public int Last { get; set; }

        public Page()
        {
            Next = 0;
            Last = 0;
        }
    }
}
