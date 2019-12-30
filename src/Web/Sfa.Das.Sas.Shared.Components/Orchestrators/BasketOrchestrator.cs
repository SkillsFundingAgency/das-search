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
        private readonly IApprenticehipFavouritesBasketStoreConfig _config;

        public BasketOrchestrator(IMediator mediator, ICookieManager cookieManager, IBasketViewModelMapper basketViewModelMapper, ICacheStorageService cacheService)
        {
            _mediator = mediator;
            _cookieManager = cookieManager;
            _basketViewModelMapper = basketViewModelMapper;
            _cacheService = cacheService;
        }

        public async Task<BasketViewModel<ApprenticeshipBasketItemViewModel>> GetBasket(Guid basketId)
        {
            var cacheKey = $"cachedBasket-{basketId.ToString()}";
            var cachedBasket = await _cacheService.RetrieveFromCache<BasketViewModel<ApprenticeshipBasketItemViewModel>>(cacheKey);

            if (cachedBasket == null)
            {
                var basket = await _mediator.Send(new GetBasketQuery { BasketId = basketId });

                return _basketViewModelMapper.Map(basket, basketId);
            }

            return cachedBasket;
        }

        public async Task<BasketViewModel<ApprenticeshipBasketItemViewModel>> GetBasket()
        {
            // Get cookie
            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?) result : null;

            if (cookieBasketId.HasValue)
            {
                return await GetBasket(cookieBasketId.Value);
            }
            else
            {
                return new BasketViewModel<ApprenticeshipBasketItemViewModel>();
            }
        }

        public async Task UpdateApprenticeship(string apprenticeshipId, int? ukprn = null, int? locationId = null)
        {
            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            var cacheKey = $"cacheKey-{cookieBasketId}";

            var basketId = await _mediator.Send(new AddOrRemoveFavouriteInBasketCommand
            {
                ApprenticeshipId = apprenticeshipId,
                Ukprn = ukprn,
                BasketId = cookieBasketId,
                LocationId = locationId
            });



            _cookieManager.Set(CookieNames.BasketCookie, basketId.ToString(), DateTime.Now.AddDays(30));   //_config.BasketSlidingExpiryDays));
        }

       
    }
}