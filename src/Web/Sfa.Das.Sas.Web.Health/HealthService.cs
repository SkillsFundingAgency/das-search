namespace Sfa.Das.Sas.Web.Health
{
    using System.Net;
    using Models;

    public interface IHealthService
    {
        HealthModel CreateModel();
    }

    public class HealthService : IHealthService
    {
        private readonly IHealthSettings _healthSettings;

        public HealthService(IHealthSettings healthSettings)
        {
            _healthSettings = healthSettings;
        }

        public HealthModel CreateModel()
        {
            return new HealthModel
            {
                WebAppVersion = string.Empty,
                PostcodeIoStatus = GetStatus(_healthSettings.PostcodeUrl + "/N170AP"),
                ApiStatus = GetStatus(_healthSettings.ApprenticeshipApiBaseUrl)
            };
        }

        private Status GetStatus(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = false;
            request.Method = "HEAD";
            try
            {
                using (request.GetResponse())
                {
                    return Status.Green;
                }
            }
            catch (WebException)
            {
                return Status.Red;
            }
        }
    }
}
