namespace Sfa.Das.WebTest.Infrastructure.Hooks
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Net;

    public class DeploymentValidator
    {
        static string applicationVersion = ConfigurationManager.AppSettings["application.version"];
        static string url = ConfigurationManager.AppSettings["service.url"];

        public static void WaitForDeployment()
        {
            if (!string.IsNullOrEmpty(applicationVersion))
            {
                var client = new WebClient();

                var version = client.DownloadString(url + "/api/version").Replace("\"", "");

                if (version != applicationVersion)
                {
                    if (!Retry.DoUntil(() => client.DownloadString(url + "/api/version").Replace("\"", "") == applicationVersion, TimeSpan.FromSeconds(3)))
                    {
                        if (version != applicationVersion)
                        {
                            throw new VersionNotFoundException($"site was version {version} but we expected {applicationVersion}");
                        }
                    }
                }
            }
        }
    }
}