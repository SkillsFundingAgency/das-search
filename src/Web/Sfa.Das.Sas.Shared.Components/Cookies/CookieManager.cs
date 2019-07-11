using Microsoft.AspNetCore.Http;

namespace Sfa.Das.Sas.Shared.Components.Cookies
{
    public class CookieManager : ICookieManager
    {
        private IHttpContextAccessor _contextAccessor;

        public CookieManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string Get(string cookieName)
        {
            return _contextAccessor.HttpContext.Request.Cookies[cookieName];
        }

        public void Set(string cookieName, string value)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append(cookieName, value);
        }
    }
}
