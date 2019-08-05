using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class ApprenticeshipFavouriteWrite
    {
        public ApprenticeshipFavouriteWrite()
        {
            Ukprns = new List<int>();
        }

        public ApprenticeshipFavouriteWrite(string apprenticeshipId) : this()
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public ApprenticeshipFavouriteWrite(string apprenticeshipId, int ukprn) : this(apprenticeshipId)
        {
            Ukprns.Add(ukprn);
        }

        public string ApprenticeshipId { get; set; }
        public IList<int> Ukprns { get; set; }
    }
}
