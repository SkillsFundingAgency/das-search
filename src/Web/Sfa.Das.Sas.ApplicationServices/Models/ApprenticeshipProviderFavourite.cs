using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class ApprenticeshipProviderFavourite
    {
        public ApprenticeshipProviderFavourite(int ukprn, IList<int> locations)
        {
            Ukprn = ukprn;
            Locations = locations;
        }

        public int Ukprn { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> Locations { get; set; }
    }
}