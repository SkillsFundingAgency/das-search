namespace Sfa.Das.Sas.Web.Factories.Interfaces
{
    using System.Web;

    public interface IHttpCookieFactory
    {
        HttpCookieCollection GetRequestCookies();
        HttpCookieCollection GetResponseCookies();
    }
}
