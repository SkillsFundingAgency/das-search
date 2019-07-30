using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.ViewModels.Basket;
using System;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class BasketController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ICookieManager _cookieManager;
        private readonly IApprenticehipFavouritesBasketStoreConfig _config;

        public BasketController(
            IMediator mediator, 
            ICookieManager cookieManager, 
            IApprenticehipFavouritesBasketStoreConfig config)
        {
            _mediator = mediator;
            _cookieManager = cookieManager;
            _config = config;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddApprenticeshipFromDetails(string apprenticeshipId)
        {
            await SaveApprenticeship(apprenticeshipId);

            return RedirectToAction("Apprenticeship", "Fat", new { id = apprenticeshipId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddApprenticeshipFromResults(SaveBasketFromApprenticeshipResultsViewModel queryModel)
        {
            await SaveApprenticeship(queryModel.ApprenticeshipId);

            return RedirectToAction("Search", "Fat", queryModel.SearchQuery);
        }

        private async Task SaveApprenticeship(string apprenticeshipId)
        {
            // Get cookie
            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            var basketId = await _mediator.Send(new AddFavouriteToBasketCommand
            {
                ApprenticeshipId = apprenticeshipId,
                BasketId = cookieBasketId
            });

            _cookieManager.Set(CookieNames.BasketCookie, basketId.ToString(), DateTime.Now.AddDays(_config.BasketSlidingExpiryDays));
        }
    }
}
