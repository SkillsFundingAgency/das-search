using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class ApprenticeshipFavouriteRead
    {
        public ApprenticeshipFavouriteRead()
        {
            Providers = new List<ApprenticeshipProviderFavourite>();
        }

        public ApprenticeshipFavouriteRead(string apprenticeshipId) : this()
        {
            ApprenticeshipId = apprenticeshipId;
        }
        public string ApprenticeshipId { get; set; }
        public IList<ApprenticeshipProviderFavourite> Providers { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int Level { get; set; }
    }

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