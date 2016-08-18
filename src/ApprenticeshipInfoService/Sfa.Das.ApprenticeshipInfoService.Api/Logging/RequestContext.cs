using System.Web;
using Sfa.Das.ApprenticeshipInfoService.Core.Logging;

namespace Sfa.Das.ApprenticeshipInfoService.Api.Logging
{
    public sealed class RequestContext : IRequestContext
    {
        public RequestContext(HttpContextBase context)
        {
            try
            {
                IpAddress = context?.Request.UserHostAddress;
                Url = context?.Request.RawUrl;
            }
            catch (HttpException)
            {
                // Happens on request that starts the application.
            }
        }

        public string IpAddress { get; private set; }

        public string Url { get; private set; }
    }
}