namespace Sfa.Das.Sas.Web.Services
{
    using System;
    using System.Web;

    using Core.Configuration;

    public class CookieService : ICookieService
    {
        private readonly IConfigurationSettings _applicationSettings;

        public CookieService(IConfigurationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public bool ShowCookieForBanner(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                return false;
            }

            var httpCookie = httpContext.Request.Cookies.Get(_applicationSettings.CookieInfoBannerCookieName);

            if (httpCookie != null)
            {
                return false;
            }

            var cookie = new HttpCookie(_applicationSettings.CookieInfoBannerCookieName)
            {
                Expires = DateTime.UtcNow.AddDays(30)
            };

            httpContext.Response.Cookies.Add(cookie);

            return true;
        }
    }
}