using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Core.Logging.Models
{
    public class TimingLogEntry : ILogEntry
    {
        public string Name => "Timing";

        public double ElaspedMilliseconds { get; set; }
    }
}
