namespace Sfa.Das.Sas.Web.Services
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Sfa.Das.Sas.Web.Settings;

    public class CookieService : ICookieService
    {
        private readonly GeneralSettings _applicationSettings;
        private readonly IHttpContextAccessor _contextAccessor;

        public CookieService(IOptions<GeneralSettings> applicationSettings, IHttpContextAccessor contextAccessor)
        {
            _applicationSettings = applicationSettings.Value;
            _contextAccessor = contextAccessor;
        }

        public bool ShowCookieForBanner()
        {
            var httpCookie = _contextAccessor.HttpContext.Request.Cookies[_applicationSettings.CookieInfoBannerCookieName];

            if (httpCookie != null)
            {
                return false;
            }

            CookieOptions cookieOptions = new CookieOptions { Expires = DateTime.UtcNow.AddDays(30) };
            _contextAccessor.HttpContext.Response.Cookies.Append(_applicationSettings.CookieInfoBannerCookieName, "", cookieOptions);

            return true;
        }
    }
}