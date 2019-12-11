using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.Shared.Basket.Models
{
    public class ApprenticeshipFavouritesBasket : IEnumerable<ApprenticeshipFavourite>
    {
        private List<ApprenticeshipFavourite> _items = new List<ApprenticeshipFavourite>();

        public ApprenticeshipFavouritesBasket()
        {
            Id = Guid.NewGuid();
        }

        internal ApprenticeshipFavouritesBasket(IList<ApprenticeshipFavourite> basketItems)
        {
            _items.AddRange(basketItems);
        }

        public ApprenticeshipFavouritesBasket(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        public bool Add(string apprenticeshipId)    
        {
            if (IsInBasket(apprenticeshipId))
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
            if (!IsInBasket(apprenticeshipId))
            {
                _items.Add(new ApprenticeshipFavourite(apprenticeshipId, ukprn));
                return true;
            }

            if (IsInBasket(apprenticeshipId,ukprn))
            {
                return false;
            }

            _items.FirstOrDefault(x => x.ApprenticeshipId == apprenticeshipId)?.Providers.Add(ukprn,new List<int>());

            return true;
        }

        public bool Add(string apprenticeshipId, int ukprn, int location)
        {
            if (!IsInBasket(apprenticeshipId))
            {
                _items.Add(new ApprenticeshipFavourite(apprenticeshipId, ukprn, location));
                return true;
            }

            if (IsInBasket(apprenticeshipId, ukprn, location))
            {
                return false;
            }

            if (IsInBasket(apprenticeshipId,ukprn))
            {
                var item = _items.FirstOrDefault(x => x.ApprenticeshipId == apprenticeshipId)?.Providers[ukprn];

                item.Add(location);
            }
            else
            {
                _items.FirstOrDefault(x => x.ApprenticeshipId == apprenticeshipId)?.Providers.Add(ukprn, new List<int>(){location});
            }

            return true;
        }

        public bool IsInBasket(string apprenticeshipId, int ukprn, int location)
        {
            if (IsInBasket(apprenticeshipId, ukprn))
            {
                var provider = _items.FirstOrDefault(x => x.ApprenticeshipId == apprenticeshipId)?.Providers[ukprn];

                return provider.Contains(location);
            }

            return false;
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
                    return apprenticeship.Providers.Any(w => w.Key == ukprn);
                }
            }
        }

        public void Remove(string apprenticeshipId)
        {
            _items.RemoveAll(w => w.ApprenticeshipId == apprenticeshipId);
        }

        public void Remove(string apprenticeshipId, int ukprn)
        {
            _items.FirstOrDefault(w => w.ApprenticeshipId == apprenticeshipId)?.Providers.Remove(ukprn);
        }

        public void Remove(string apprenticeshipId, int ukprn, int location)
        {
            var apprenticeship = _items.FirstOrDefault(x => x.ApprenticeshipId == apprenticeshipId);
            var provider = apprenticeship?.Providers[ukprn];
            provider?.Remove(location);
            
            if (provider.Count == 0)
                apprenticeship.Providers.Remove(ukprn);
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
