using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Components.Cookies;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Basket
{
    public class AddToBasketViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly ICookieManager _cookieManager;

        public AddToBasketViewComponent(IMediator mediator, ICookieManager cookieManager)
        {
            _mediator = mediator;
            _cookieManager = cookieManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string apprenticeshipId, int? ukprn = null)
        {
            var model = new AddToBasketViewModel
            {
                ItemId = ukprn.HasValue ? ukprn.ToString() : apprenticeshipId,
                IsInBasket = await IsInBasket(apprenticeshipId, ukprn)
            };

            return View("../Basket/AddToBasket/Default", model);
        }

        private async Task<bool> IsInBasket(string apprenticeshipId, int? ukprn)
        {
            // Get cookie
            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            if (cookieBasketId.HasValue)
            {
                var basket = await _mediator.Send(new GetBasketQuery { BasketId = cookieBasketId.Value });

                return basket.IsInBasket(apprenticeshipId, ukprn);
            }

            return false;
        }
    }
}
