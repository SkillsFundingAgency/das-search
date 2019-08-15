namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class ApprenticeshipProviderFavourite
    {
        public ApprenticeshipProviderFavourite(int ukprn)
        {
            Ukprn = ukprn;
        }

        public int Ukprn { get; set; }
        public string Name { get; set; }
    }
}