namespace Sfa.Das.ApprenticeshipInfoService.Core.Logging
{
    public interface IRequestContext
    {
        string Url { get; }
        string IpAddress { get; }
    }
}
