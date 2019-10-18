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
        private readonly IApprenticehipFavouritesBasketStoreConfig _config;
        private readonly IBasketOrchestrator _basketOrchestrator;

        public BasketController(
            IMediator mediator,
            ICookieManager cookieManager,
            IApprenticehipFavouritesBasketStoreConfig config)
        {
            _mediator = mediator;
            _cookieManager = cookieManager;
            _config = config;
        }

        public async Task<IActionResult> View()
        {
            return View("Basket/View");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddApprenticeshipFromDetails(SaveBasketFromApprenticeshipDetailsViewModel queryModel)
        {
            if (!await IsInBasket(queryModel.ItemId, null))
            {
                await UpdateApprenticeship(queryModel.ItemId);
            }

            return RedirectToAction("Apprenticeship", "Fat", new { id = queryModel.ItemId });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddApprenticeshipFromResults(SaveBasketFromApprenticeshipResultsViewModel queryModel)
        {
            if (!await IsInBasket(queryModel.ItemId, null))
            {
                await UpdateApprenticeship(queryModel.ItemId);
            }

            return RedirectToAction("Search", "Fat", queryModel.SearchQuery);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddProviderFromDetails(SaveBasketFromProviderDetailsViewModel queryModel)
        {
            if (!await IsInBasket(queryModel.ApprenticeshipId, queryModel.ItemId))
            {
                await UpdateApprenticeship(queryModel.ApprenticeshipId, queryModel.ItemId);
            }
            return RedirectToAction("Details", "TrainingProvider", queryModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddProviderFromResults(SaveBasketFromProviderSearchViewModel queryModel)
        {
            if (!await IsInBasket(queryModel.SearchQuery.ApprenticeshipId, queryModel.ItemId))
            {
                await UpdateApprenticeship(queryModel.SearchQuery.ApprenticeshipId, queryModel.ItemId);
            }
            

            return RedirectToAction("Search", "TrainingProvider", queryModel.SearchQuery);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RemoveFromBasket(DeleteFromBasketViewModel model)
        {
            await UpdateApprenticeship(model.ApprenticeshipId, model.Ukprn);

            return RedirectToAction("View", "Basket");
        }

        private async Task UpdateApprenticeship(string apprenticeshipId, int? ukprn = null)
        {
            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            var basketId = await _mediator.Send(new AddOrRemoveFavouriteInBasketCommand
            {
                ApprenticeshipId = apprenticeshipId,
                Ukprn = ukprn,
                BasketId = cookieBasketId
            });

            _cookieManager.Set(CookieNames.BasketCookie, basketId.ToString(), DateTime.Now.AddDays(_config.BasketSlidingExpiryDays));
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
