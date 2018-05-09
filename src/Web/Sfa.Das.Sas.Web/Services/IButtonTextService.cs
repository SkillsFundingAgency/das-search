using System.Web;

namespace Sfa.Das.Sas.Web.Services
{
    public interface IButtonTextService
    {
        string GetTrainingProviderText(HttpContextBase httpContext);
    }
}