using System.Collections.Generic;

namespace Sfa.Das.Sas.Shared.Basket.Models
{
    public class ApprenticeshipFavourite
    {
        public ApprenticeshipFavourite()
        {
            Providers = new Dictionary<int, IList<int>>();
        }

        public ApprenticeshipFavourite(string apprenticeshipId) : this()
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public ApprenticeshipFavourite(string apprenticeshipId, int ukprn) : this(apprenticeshipId)
        {
            Providers.Add(new KeyValuePair<int, IList<int>>(ukprn, new List<int>()));
        }
        public ApprenticeshipFavourite(string apprenticeshipId, int ukprn, int location) : this(apprenticeshipId)
        {
            Providers.Add(new KeyValuePair<int, IList<int>>(ukprn, new List<int>(){location}));
        }

        public string ApprenticeshipId { get; set; }
        public IDictionary<int,IList<int>> Providers { get; set; }
    }
}
