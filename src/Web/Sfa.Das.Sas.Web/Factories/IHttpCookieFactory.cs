using System.Web;

namespace Sfa.Das.Sas.Web.Factories
{
    public interface IHttpCookieFactory
    {
        HttpCookieCollection GetRequestCookies();
        HttpCookieCollection GetResponseCookies();
    }
}
