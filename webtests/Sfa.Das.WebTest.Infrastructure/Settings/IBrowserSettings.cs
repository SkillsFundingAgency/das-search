namespace Sfa.Das.WebTest.Infrastructure.Settings
{
    public interface IBrowserSettings
    {
        string BaseUrl { get; }

        string Browser { get; }

        string RemoteUrl { get; }

        string BrowserStackUser { get; }

        string BrowserStackKey { get; }

        string OS { get; }

        string OSVersion { get; }

        string BrowserVersion { get; }

        string Project { get; }

        string AssemblyVersion { get; }

        string Device { get; }

        string Resolution { get; }

        string Build { get; }
    }
}