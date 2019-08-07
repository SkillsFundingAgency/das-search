using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.Shared.Basket.Models
{
    public class ApprenticeshipFavouritesBasket : IEnumerable<ApprenticeshipFavourite>
    {
        private readonly List<ApprenticeshipFavourite> _items = new List<ApprenticeshipFavourite>();

        public ApprenticeshipFavouritesBasket()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public IEnumerator<ApprenticeshipFavourite> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Add(string apprenticeshipId)
        {
            if (_items.Any(x => x.ApprenticeshipId == apprenticeshipId))
            {
                return false;
            }
            else
            {
                _items.Add(new ApprenticeshipFavourite(apprenticeshipId));

                return true;
            }
        }

        public bool Add(string apprenticeshipId, int ukprn)
        {
            var apprenticeship = _items.FirstOrDefault(x => x.ApprenticeshipId == apprenticeshipId);

            if (apprenticeship == null)
            {
                _items.Add(new ApprenticeshipFavourite(apprenticeshipId, ukprn));
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ApprenticeshipFavourite this[int i] => _items[i];
    }
}
