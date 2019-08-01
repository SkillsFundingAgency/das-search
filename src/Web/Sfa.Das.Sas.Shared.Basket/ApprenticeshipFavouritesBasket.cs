using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.Shared.Basket.Models
{
    public class ApprenticeshipFavouritesBasket : List<ApprenticeshipFavourite>
    {
        public ApprenticeshipFavouritesBasket()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public bool Update(string apprenticeshipId)
        {
            if (this.Any(x => x.ApprenticeshipId == apprenticeshipId))
            {
                return false;
            }
            else
            {
                this.Add(new ApprenticeshipFavourite(apprenticeshipId));

                return true;
            }
        }

        public bool Update(string apprenticeshipId, int ukprn)
        {
            var apprenticeship = this.FirstOrDefault(x => x.ApprenticeshipId == apprenticeshipId);

            if (apprenticeship == null)
            {
                this.Add(new ApprenticeshipFavourite(apprenticeshipId, ukprn));
                return true;
            }

            if (apprenticeship.Ukprns.Contains(ukprn))
            {
                return false;
            }
            else
            {
                apprenticeship.Ukprns.Add(ukprn);

                return true;
            }
        }
    }
}
