using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Core.Logging.Metrics
{
    public static class ExecutionTimer
    {
        public static ExecutionTimerResult<T> GetTiming<T>(Func<T> function)
        {
            var timerResult = default(ExecutionTimerResult<T>);

            var timer = Stopwatch.StartNew();
            timerResult.Result = function.Invoke();
            timerResult.ElaspedMilliseconds = timer.Elapsed.TotalMilliseconds;

            return timerResult;
        }

        public static TimeSpan GetTiming(Action action)
        {
            var timer = Stopwatch.StartNew();
            action.Invoke();
            return timer.Elapsed;
        }
    }
}
