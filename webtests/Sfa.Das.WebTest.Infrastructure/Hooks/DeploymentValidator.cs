namespace Sfa.Das.WebTest.Infrastructure.Hooks
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Net;
    using System.Reflection;

    using Newtonsoft.Json;

    public class DeploymentValidator
    {
        static readonly string AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        static string url = ConfigurationManager.AppSettings["service.url"];

        public static void WaitForDeployment()
        {
            if (!string.IsNullOrEmpty(AssemblyVersion))
            {
                var client = new WebClient();

                var version = FindVersion(client);

                if (version != AssemblyVersion)
                {
                    if (!Retry.DoUntil(() => FindVersion(client) == AssemblyVersion, TimeSpan.FromSeconds(3)))
                    {
                        if (version != AssemblyVersion)
                        {
                            throw new VersionNotFoundException($"site was version {version} but we expected {AssemblyVersion}");
                        }
                    }
                }
            }
        }

        public static string FindVersion(WebClient client)
        {
            var json = client.DownloadString(url + "/api/version");
            var versionInformation = JsonConvert.DeserializeObject<VersionInformation>(json);
            return versionInformation.AssemblyVersion;
        }
    }
}