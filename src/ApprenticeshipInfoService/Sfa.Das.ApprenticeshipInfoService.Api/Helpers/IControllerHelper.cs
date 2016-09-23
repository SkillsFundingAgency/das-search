using System.Net.Http;

namespace Sfa.Das.ApprenticeshipInfoService.Api.Helpers
{
    public interface IControllerHelper
    {
        int GetPageFromUrl(HttpRequestMessage requestMessage);
    }
}