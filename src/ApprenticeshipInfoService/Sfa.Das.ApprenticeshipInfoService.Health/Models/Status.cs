namespace Sfa.Das.ApprenticeshipInfoService.Health.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        Green = 0, Red = 1
    }
}