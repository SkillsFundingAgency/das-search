using System.Web;

namespace Sfa.Das.Sas.Web.Services
{
    public class ButtonTextService: IButtonTextService
    {
        public string GetTrainingProviderText(HttpContextBase httpContext)
        {
            var isApprenticeSearch = httpContext.Request.UrlReferrer == null || !httpContext.Request.UrlReferrer.AbsolutePath.ToLower().Contains("provider/");
            return isApprenticeSearch ? "Find Training Providers" : "Find More Training Providers";
        }
    }
}