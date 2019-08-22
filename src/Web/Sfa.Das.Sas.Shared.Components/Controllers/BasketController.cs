//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using Sfa.Das.Sas.ApplicationServices.Commands;
//using Sfa.Das.Sas.Core.Configuration;
//using Sfa.Das.Sas.Shared.Components.Cookies;
//using Sfa.Das.Sas.Shared.Components.ViewModels.Basket;
//using System;
//using System.Threading.Tasks;

//namespace Sfa.Das.Sas.Shared.Components.Controllers
//{
//    public class BasketController : Controller
//    {
//        private readonly IMediator _mediator;
//        private readonly ICookieManager _cookieManager;
//        private readonly IApprenticehipFavouritesBasketStoreConfig _config;

//        public BasketController(
//            IMediator mediator, 
//            ICookieManager cookieManager, 
//            IApprenticehipFavouritesBasketStoreConfig config)
//        {
//            _mediator = mediator;
//            _cookieManager = cookieManager;
//            _config = config;
//        }

//        [ValidateAntiForgeryToken]
//        [HttpPost]
//        public async Task<IActionResult> AddApprenticeshipFromDetails(SaveBasketFromApprenticeshipDetailsViewModel queryModel)
//        {
//            await SaveApprenticeship(queryModel.ItemId);

//            return RedirectToAction("Apprenticeship", "Fat", new { id = queryModel.ItemId });
//        }

//        [ValidateAntiForgeryToken]
//        [HttpPost]
//        public async Task<IActionResult> AddApprenticeshipFromResults(SaveBasketFromApprenticeshipResultsViewModel queryModel)
//        {
//            await SaveApprenticeship(queryModel.ItemId);

//            return RedirectToAction("Search", "Fat", queryModel.SearchQuery);
//        }

//        [ValidateAntiForgeryToken]
//        [HttpPost]
//        public async Task<IActionResult> AddProviderFromDetails(SaveBasketFromProviderDetailsViewModel queryModel)
//        {
//            await SaveApprenticeship(queryModel.ApprenticeshipId, queryModel.ItemId);

//            return RedirectToAction("Details", "TrainingProvider", queryModel);
//        }

//        [ValidateAntiForgeryToken]
//        [HttpPost]
//        public async Task<IActionResult> AddProviderFromResults(SaveBasketFromProviderSearchViewModel queryModel)
//        {
//            await SaveApprenticeship(queryModel.SearchQuery.ApprenticeshipId, queryModel.ItemId);

//            return RedirectToAction("Search", "TrainingProvider", queryModel.SearchQuery);
//        }

//        private async Task SaveApprenticeship(string apprenticeshipId, int? ukprn = null)
//        {
//            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
//            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

//            var basketId = await _mediator.Send(new AddFavouriteToBasketCommand
//            {
//                ApprenticeshipId = apprenticeshipId,
//                Ukprn = ukprn,
//                BasketId = cookieBasketId
//            });

//            _cookieManager.Set(CookieNames.BasketCookie, basketId.ToString(), DateTime.Now.AddDays(_config.BasketSlidingExpiryDays));
//        }
//    }
//}
