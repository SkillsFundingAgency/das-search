using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.ViewModels.Apprenticeship;
using Sfa.Das.Sas.Shared.Components.ViewModels.Basket;

namespace Sfa.Das.Sas.Shared.Components.Orchestrators
{
    public class BasketOrchestrator : IBasketOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ICookieManager _cookieManager;
        private readonly IBasketViewModelMapper _basketViewModelMapper;
        private readonly ICacheStorageService _cacheService;
        private readonly ICacheSettings _cacheSettings;

        public BasketOrchestrator(IMediator mediator, ICookieManager cookieManager, IBasketViewModelMapper basketViewModelMapper, ICacheStorageService cacheService, ICacheSettings cacheSettings)
        {
            _mediator = mediator;
            _cookieManager = cookieManager;
            _basketViewModelMapper = basketViewModelMapper;
            _cacheService = cacheService;
            _cacheSettings = cacheSettings;
        }

        public async Task<BasketViewModel<ApprenticeshipBasketItemViewModel>> GetBasket(Guid basketId)
        {
            return await GetBasket(basketId, true);
        }

        private async Task<BasketViewModel<ApprenticeshipBasketItemViewModel>> GetBasket(Guid basketId, bool fromCache)
        {
            if (fromCache)
            {
                var cacheKey = $"cachedBasket-{basketId.ToString()}";
                var cachedBasket = await _cacheService.RetrieveFromCache<BasketViewModel<ApprenticeshipBasketItemViewModel>>(cacheKey);

                if (cachedBasket != null)
                {
                    return cachedBasket;
                }
            }

            var basket = await _mediator.Send(new GetBasketQuery { BasketId = basketId });

            return _basketViewModelMapper.Map(basket, basketId);
        }

        public async Task<BasketViewModel<ApprenticeshipBasketItemViewModel>> GetBasket()
        {
            // Get cookie

            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            if (cookieBasketId.HasValue)
            {
                return await GetBasket(cookieBasketId.Value);
            }
            else
            {
                return new BasketViewModel<ApprenticeshipBasketItemViewModel>();
            }
        }

        public async Task UpdateBasket(string apprenticeshipId, int? ukprn = null, int? locationId = null)
        {
            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            var basketId = await _mediator.Send(new AddOrRemoveFavouriteInBasketCommand
            {
                ApprenticeshipId = apprenticeshipId,
                Ukprn = ukprn,
                BasketId = cookieBasketId,
                LocationId = locationId
            });

            _cookieManager.Set(CookieNames.BasketCookie, basketId.ToString(), DateTime.Now.AddDays(30));

            await _cacheService.SaveToCache($"cachedBasket-{basketId.ToString()}", await GetBasket(basketId, false), new TimeSpan(_cacheSettings.CacheAbsoluteExpirationDays, 0, 0, 0), new TimeSpan(_cacheSettings.CacheSlidingExpirationDays, 0, 0, 0));
        }
    }
}