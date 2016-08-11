using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.ApplicationServices.Http
{
    public interface IRetryWebRequest
    {
        T RetryWeb<T>(Func<T> action);
    }
}
