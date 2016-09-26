namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    public class Location
    {
        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public Address Address { get; set; }
    }
}
