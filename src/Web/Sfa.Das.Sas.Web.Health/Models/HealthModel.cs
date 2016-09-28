namespace Sfa.Das.Sas.Web.Health.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class HealthModel
    {
        public string WebAppVersion { get; set; }

        public Status PostcodeIoStatus { get; set; }

        public Status ApiStatus { get; set; }

        public Status ApplicationStatus {
            get
            {
                if (PostcodeIoStatus == Status.Red || ApiStatus == Status.Red)
                {
                    return Status.Red;
                }

                return Status.Green;
            }
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        Green = 0, Red = 1
    }
}