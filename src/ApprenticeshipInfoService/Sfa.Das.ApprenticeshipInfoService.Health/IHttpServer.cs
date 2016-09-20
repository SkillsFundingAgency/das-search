namespace Sfa.Das.ApprenticeshipInfoService.Health
{
    public interface IHttpServer
    {
        string ResponseCode(string url);

        string Ping(string url);

        string GetData(string url);
    }
}