using System.Web;

namespace Sfa.Das.Sas.Web.Services
{
    public interface IButtonTextService
    {
        string GetFindTrainingProvidersText(HttpContextBase httpContext);
    }
}