namespace Sfa.Das.ApprenticeshipInfoService.Core.Models
{
    public sealed class Provider
    {
        public int UkPrn { get; set; }

        public string Name { get; set; }

        public bool NationalProvider { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}
