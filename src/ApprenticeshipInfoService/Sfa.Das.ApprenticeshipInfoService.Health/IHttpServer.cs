namespace Sfa.Das.ApprenticeshipInfoService.Health
{
    using Sfa.Das.ApprenticeshipInfoService.Health.Models;

    public interface IHttpServer
    {
        Status ResponseCode(string url);

        Status GetStatus(string url);
    }
}