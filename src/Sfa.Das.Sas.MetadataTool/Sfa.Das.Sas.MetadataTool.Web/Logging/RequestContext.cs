namespace Sfa.Das.Sas.MetadataTool.Web.Logging
{
    using System.Web;

    using Sfa.Das.Sas.Core.Logging;

    public sealed class RequestContext : IRequestContext
    {
        public RequestContext(HttpContextBase context)
        {
            try
            {
                this.IpAddress = context?.Request.UserHostAddress;
                this.Url = context?.Request.RawUrl;
            }
            catch (HttpException)
            {
                // Happens on request that starts the application. ¯\_(ツ)_/¯
            }
        }

        public string IpAddress { get; private set; }
        public string Url { get; private set; }
    }
}