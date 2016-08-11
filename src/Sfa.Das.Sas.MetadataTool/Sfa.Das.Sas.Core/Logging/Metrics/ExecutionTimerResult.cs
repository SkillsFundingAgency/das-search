using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Core.Logging.Metrics
{
    public struct ExecutionTimerResult<T>
    {
        public T Result { get; set; }

        public double ElaspedMilliseconds { get; set; }
    }
}
