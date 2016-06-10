using System.Web;

namespace Sfa.Das.Sas.Web.Factories
{
    using Sfa.Das.Sas.Web.Factories.Interfaces;

    public class HttpCookieFactory : IHttpCookieFactory
    {
        public HttpCookieCollection GetRequestCookies()
        {
            return HttpContext.Current.Request.Cookies;
        }

        public HttpCookieCollection GetResponseCookies()
        {
            return HttpContext.Current.Response.Cookies;
        }
    }
}