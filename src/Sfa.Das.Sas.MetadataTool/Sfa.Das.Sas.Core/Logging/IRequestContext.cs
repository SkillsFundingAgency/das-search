namespace Sfa.Das.Sas.Core.Logging
{
    public interface IRequestContext
    {
        string Url { get; }
        string IpAddress { get; }
    }
}
