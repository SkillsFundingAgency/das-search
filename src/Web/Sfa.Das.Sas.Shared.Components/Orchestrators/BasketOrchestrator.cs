using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.ViewModels.Basket;

namespace Sfa.Das.Sas.Shared.Components.Orchestrators
{
    public class BasketOrchestrator : IBasketOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ICookieManager _cookieManager;
        private readonly IBasketViewModelMapper _basketViewModelMapper;

        public BasketOrchestrator(IMediator mediator, ICookieManager cookieManager, IBasketViewModelMapper basketViewModelMapper)
        {
            _mediator = mediator;
            _cookieManager = cookieManager;
            _basketViewModelMapper = basketViewModelMapper;
        }

        public async Task<BasketViewModel> GetBasket(Guid basketId)
        {
                var basket = await _mediator.Send(new GetBasketQuery { BasketId = basketId });

                return _basketViewModelMapper.Map(basket);
        }

        public async Task<BasketViewModel> GetBasket()
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
                return new BasketViewModel();
            }
        }
    }
}