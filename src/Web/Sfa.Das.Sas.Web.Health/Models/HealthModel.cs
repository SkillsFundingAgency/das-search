namespace Sfa.Das.Sas.Web.Health.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class HealthModel
    {

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

        public string BuildId { get; set; }

        public string Version { get; set; }

        public string AssemblyVersion { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        Green = 0, Red = 1
    }
}