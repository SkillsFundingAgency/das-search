using Microsoft.AspNetCore.Http;

namespace Sfa.Das.Sas.Web.Services
{
    public interface IButtonTextService
    {
        string GetFindTrainingProvidersText(IHttpContextAccessor httpContext);
    }
}