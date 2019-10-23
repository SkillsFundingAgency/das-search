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

        internal ApprenticeshipFavouritesBasket(IList<ApprenticeshipFavourite> basketItems)
        {
            _items.AddRange(basketItems);
        }

        public Guid Id { get; set; }

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

        public bool IsInBasket(string apprenticeshipId, int? ukprn = null)
        {
            if (!ukprn.HasValue)
            {
                return _items.Any(x => x.ApprenticeshipId == apprenticeshipId);
            }
            else
            {
                var apprenticeship = _items.SingleOrDefault(x => x.ApprenticeshipId == apprenticeshipId);

                if (apprenticeship == null)
                    return false;
                else
                {
                    return ukprn.HasValue ? apprenticeship.Ukprns.Contains(ukprn.Value) : false;
                }
            }
        }

        public void Remove(string apprenticeshipId)
        {
            _items.RemoveAll(w => w.ApprenticeshipId == apprenticeshipId);
        }

        public void Remove(string apprenticeshipId, int ukprn)
        {
            _items.FirstOrDefault(w => w.ApprenticeshipId == apprenticeshipId)?.Ukprns.Remove(ukprn);
        }
        public IEnumerator<ApprenticeshipFavourite> GetEnumerator() 
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public ApprenticeshipFavourite this[int i] => _items[i];
    }
}
