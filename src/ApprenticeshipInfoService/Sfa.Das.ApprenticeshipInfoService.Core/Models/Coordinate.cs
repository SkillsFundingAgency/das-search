namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    public sealed class Coordinate
    {
        public double Lat { get; set; }

        public double Lon { get; set; }

        public override string ToString()
        {
            return $"Longitude: {Lon}, Latitude: {Lat}";
        }
    }
}
