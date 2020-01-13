using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.ViewModels.Basket;
using System;
using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Components.Orchestrators;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class BasketController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ICookieManager _cookieManager;

        public BasketController(
            IMediator mediator,
            ICookieManager cookieManager)
        {
            _mediator = mediator;
            _cookieManager = cookieManager;

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ClearBasket()
        {
            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            await _mediator.Send(new RemoveBasketCommand
            {
                BasketId = cookieBasketId
            });

            return RedirectToAction("View", "Basket");
        }

    }
}
