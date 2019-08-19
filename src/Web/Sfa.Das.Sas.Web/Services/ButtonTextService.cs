using System;
using Microsoft.AspNetCore.Http;

namespace Sfa.Das.Sas.Web.Services
{
    public class ButtonTextService : IButtonTextService
    {
        public string GetFindTrainingProvidersText(IHttpContextAccessor httpContext)
        {
            string referer = httpContext.HttpContext.Request.Headers["Referer"];
            var isApprenticeSearch = referer == null || !new Uri(referer).AbsolutePath.ToLower().Contains("provider/");
            return isApprenticeSearch ? "Find training providers" : "Find more training providers";
        }
    }
}