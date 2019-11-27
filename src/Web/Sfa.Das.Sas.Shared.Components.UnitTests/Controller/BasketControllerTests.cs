using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Shared.Components.Controllers;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.ViewModels.Basket;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Basket.Models;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Controller
{
    [TestFixture]
    public class BasketControllerTests
    {
        private const string APPRENTICESHIP_ID = "123";
        private const int UKPRN = 12345678;
        private const int LOCATION_ID = 123123;
        private const int LOCATION_ID_TO_ADD = 5555;
        private const string BasketCookieName = "ApprenticeshipBasket";
        private Mock<IMediator> _mockMediator;
        private Mock<ICookieManager> _mockCookieManager;
        private BasketController _sut;

        private SaveBasketFromApprenticeshipDetailsViewModel _addFromApprenticeshipDetailsModel;
        private SaveBasketFromApprenticeshipResultsViewModel _addFromApprenticeshipResultsModel;
        private SaveBasketFromProviderDetailsViewModel _addFromProviderDetailsModel;
        private SaveBasketFromProviderSearchViewModel _addFromProviderSearchModel;
        private DeleteFromBasketViewModel _deleteFromBasketViewModel;

        [SetUp]
        public void Setup()
        {
            _addFromApprenticeshipDetailsModel = GetApprenticeshipDetailsRequestModel();
            _addFromApprenticeshipResultsModel = GetApprenticeshipResultsRequestModel();
            _addFromProviderDetailsModel = GetProviderDetailsRequestModel();
            _addFromProviderSearchModel = GetProviderResultsRequestModel();
            _deleteFromBasketViewModel = GetDeleteFromBasketViewModel();

            // Set cookie in http request
            _mockMediator = new Mock<IMediator>();
            _mockCookieManager = new Mock<ICookieManager>();

            _mockMediator.Setup(s => s.Send(It.IsAny<GetBasketQuery>(), default(CancellationToken))).ReturnsAsync(GetApprenticeshipFavouritesBasketRead());

            _sut = new BasketController(_mockMediator.Object, _mockCookieManager.Object, Mock.Of<IApprenticehipFavouritesBasketStoreConfig>());
        }

        #region AddApprenticeshipFromDetails

        [Test]
        public async Task AddApprenticeshipFromDetails_ReturnsRedirectResult_ToApprenticeshipDetailsPage()
        {
            var result = await _sut.AddApprenticeshipFromDetails(_addFromApprenticeshipDetailsModel);

            result.Should().BeAssignableTo<RedirectToActionResult>();
            var redirect = (RedirectToActionResult)result;

            redirect.ControllerName.Should().Be("Fat");
            redirect.ActionName.Should().Be("Apprenticeship");
            var routeValues = redirect.RouteValues;
            routeValues["id"].Should().Be(APPRENTICESHIP_ID);
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_ParsesApprenticeshipId_FromArgument()
        {
            var result = await _sut.AddApprenticeshipFromDetails(_addFromApprenticeshipDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.ApprenticeshipId == "123"), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_UsesBasketIdFromCookieForUpdate_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());
            _addFromApprenticeshipDetailsModel.ItemId = "333"; // Set to a new apprenticeship value

            var result = await _sut.AddApprenticeshipFromDetails(_addFromApprenticeshipDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddApprenticeshipFromDetails(_addFromApprenticeshipDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
            _mockCookieManager.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>()));

            var result = await _sut.AddApprenticeshipFromDetails(_addFromApprenticeshipDetailsModel);

            _mockCookieManager.Verify(x => x.Set(BasketCookieName, newBasketId.ToString(), It.IsAny<DateTimeOffset?>()));
        }

        #endregion

        #region AddApprenticeshipFromResults

        [Test]
        public async Task AddApprenticeshipFromResults_ReturnsRedirectResult_ToApprenticeshipDetailsPage()
        {
            var result = await _sut.AddApprenticeshipFromResults(_addFromApprenticeshipResultsModel);

            result.Should().BeAssignableTo<RedirectToActionResult>();
            var redirect = (RedirectToActionResult)result;

            redirect.ControllerName.Should().Be("Fat");
            redirect.ActionName.Should().Be("Search");
            var routeValues = redirect.RouteValues;
            routeValues["Keywords"].Should().Be(_addFromApprenticeshipResultsModel.SearchQuery.Keywords);
            routeValues["Page"].Should().Be(_addFromApprenticeshipResultsModel.SearchQuery.Page);
            routeValues["ResultsToTake"].Should().Be(_addFromApprenticeshipResultsModel.SearchQuery.ResultsToTake);
            routeValues["SortOrder"].Should().Be(_addFromApprenticeshipResultsModel.SearchQuery.SortOrder);
            routeValues["SelectedLevels"].Should().Be(_addFromApprenticeshipResultsModel.SearchQuery.SelectedLevels);
        }

        [Test]
        public async Task AddApprenticeshipFromResults_ParsesApprenticeshipId_FromArgument()
        {
            var result = await _sut.AddApprenticeshipFromResults(_addFromApprenticeshipResultsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.ApprenticeshipId == "123"), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromResults_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());
            _addFromApprenticeshipResultsModel.ItemId = "333"; // Set new apprenticeship value

            var result = await _sut.AddApprenticeshipFromResults(_addFromApprenticeshipResultsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromResults_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddApprenticeshipFromResults(_addFromApprenticeshipResultsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromResults_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
            _mockCookieManager.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>()));

            var result = await _sut.AddApprenticeshipFromResults(_addFromApprenticeshipResultsModel);

            _mockCookieManager.Verify(x => x.Set(BasketCookieName, newBasketId.ToString(), It.IsAny<DateTimeOffset?>()));
        }

        #endregion

        #region AddProviderFromDetails

        [Test]
        public async Task AddProviderFromDetails_ReturnsRedirectResult_ToProviderDetailsPage()
        {
            var result = await _sut.AddProviderFromDetails(_addFromProviderDetailsModel);

            result.Should().BeAssignableTo<RedirectToActionResult>();
            var redirect = (RedirectToActionResult)result;

            redirect.ControllerName.Should().Be("TrainingProvider");
            redirect.ActionName.Should().Be("Details");
            var routeValues = redirect.RouteValues;
            routeValues["ukprn"].Should().Be(UKPRN);
            routeValues["apprenticeshipId"].Should().Be(APPRENTICESHIP_ID);
            routeValues["locationId"].Should().Be(LOCATION_ID);
        }

        [Test]
        public async Task AddProviderFromDetails_ParsesApprenticeshipIdAndUkprn_FromArgument()
        {
            var result = await _sut.AddProviderFromDetails(_addFromProviderDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.ApprenticeshipId == APPRENTICESHIP_ID && a.Ukprn == UKPRN), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromDetails_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());
            _addFromProviderDetailsModel.ItemId = "33,1234"; // Set new apprenticeship value

            var result = await _sut.AddProviderFromDetails(_addFromProviderDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromDetails_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddProviderFromDetails(_addFromProviderDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromDetails_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
            _mockCookieManager.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>()));

            var result = await _sut.AddProviderFromDetails(_addFromProviderDetailsModel);

            _mockCookieManager.Verify(x => x.Set(BasketCookieName, newBasketId.ToString(), It.IsAny<DateTimeOffset?>()));
        }

        #endregion

        #region AddProviderFromResults

        [Test]
        public async Task AddProviderFromResults_ReturnsRedirectResult_ToApprenticeshipDetailsPage()
        {
            var result = await _sut.AddProviderFromResults(_addFromProviderSearchModel);

            result.Should().BeAssignableTo<RedirectToActionResult>();
            var redirect = (RedirectToActionResult)result;

            redirect.ControllerName.Should().Be("TrainingProvider");
            redirect.ActionName.Should().Be("Search");
            var routeValues = redirect.RouteValues;
            routeValues["ApprenticeshipId"].Should().Be(_addFromProviderSearchModel.SearchQuery.ApprenticeshipId);
            routeValues["IsLevyPayer"].Should().Be(_addFromProviderSearchModel.SearchQuery.IsLevyPayer);
            routeValues["NationalProvidersOnly"].Should().Be(_addFromProviderSearchModel.SearchQuery.NationalProvidersOnly);
            routeValues["Page"].Should().Be(_addFromProviderSearchModel.SearchQuery.Page);
            routeValues["Postcode"].Should().Be(_addFromProviderSearchModel.SearchQuery.Postcode);
            routeValues["SortOrder"].Should().Be(_addFromProviderSearchModel.SearchQuery.SortOrder);
        }

        [Test]
        public async Task AddProviderFromResults_ParsesApprenticeshipIdAndUkprn_FromArgument()
        {
            var result = await _sut.AddProviderFromResults(_addFromProviderSearchModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.ApprenticeshipId == APPRENTICESHIP_ID && a.Ukprn == UKPRN), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromResults_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());
            _addFromProviderSearchModel.ItemId = "33,10"; // Set new apprenticeship value

            var result = await _sut.AddProviderFromResults(_addFromProviderSearchModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromResults_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddProviderFromResults(_addFromProviderSearchModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromResults_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
            _mockCookieManager.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>()));

            var result = await _sut.AddProviderFromResults(_addFromProviderSearchModel);

            _mockCookieManager.Verify(x => x.Set(BasketCookieName, newBasketId.ToString(), It.IsAny<DateTimeOffset?>()));
        }

        #endregion


        #region RemoveFromBasket

        [Test]
        public async Task RemoveFromBasket_ReturnsRedirectResult_ToBasketPage()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());

            var result = await _sut.RemoveFromBasket(_deleteFromBasketViewModel);

            result.Should().BeAssignableTo<RedirectToActionResult>();
            var redirect = (RedirectToActionResult)result;

            redirect.ControllerName.Should().Be("Basket");
            redirect.ActionName.Should().Be("View");
        }

        [Test]
        public async Task RemoveFromBasket_ParsesApprenticeshipIdAndUkprn_FromArgument()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());

            var result = await _sut.RemoveFromBasket(_deleteFromBasketViewModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.ApprenticeshipId == APPRENTICESHIP_ID && a.Ukprn == UKPRN), default(CancellationToken)));
        }

        [Test]
        public async Task RemoveFromBasket_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());

            var result = await _sut.RemoveFromBasket(_deleteFromBasketViewModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddOrRemoveFavouriteInBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task RemoveFromBasket_RedirectToBasket_IfNoCookieExists()
        {
            var result = await _sut.RemoveFromBasket(_deleteFromBasketViewModel);

            result.Should().BeAssignableTo<RedirectToActionResult>();
            var redirect = (RedirectToActionResult)result;

            redirect.ControllerName.Should().Be("Basket");
            redirect.ActionName.Should().Be("View");
        }

        [Test]
        public async Task RemoveFromBasket_RedirectToBasket_IfItemDoesntExistInBakset()
        {
            var removeViewModel = _deleteFromBasketViewModel;

            removeViewModel.Ukprn = 456789012;

            var result = await _sut.RemoveFromBasket(removeViewModel);

            result.Should().BeAssignableTo<RedirectToActionResult>();
            var redirect = (RedirectToActionResult)result;

            redirect.ControllerName.Should().Be("Basket");
            redirect.ActionName.Should().Be("View");
        }

        #endregion
        private static SaveBasketFromApprenticeshipDetailsViewModel GetApprenticeshipDetailsRequestModel() => new SaveBasketFromApprenticeshipDetailsViewModel
        {
            ItemId = APPRENTICESHIP_ID
        };

        private static SaveBasketFromApprenticeshipResultsViewModel GetApprenticeshipResultsRequestModel() => new SaveBasketFromApprenticeshipResultsViewModel
        {
            ItemId = APPRENTICESHIP_ID,
            SearchQuery = new ViewModels.SearchQueryViewModel
            {
                Keywords = "baker",
                Page = 3,
                ResultsToTake = 40,
                SortOrder = 1
            }
        };

        private static SaveBasketFromProviderDetailsViewModel GetProviderDetailsRequestModel() => new SaveBasketFromProviderDetailsViewModel
        {
            ItemId = $"{UKPRN},{LOCATION_ID_TO_ADD}",
            ApprenticeshipId = APPRENTICESHIP_ID,
            ApprenticeshipType = ApplicationServices.Models.ApprenticeshipType.Standard,
            Page = 1,
            Ukprn = UKPRN,
            LocationId = LOCATION_ID,
            ViewType = ViewModels.ViewType.Details
        };

        private static SaveBasketFromProviderSearchViewModel GetProviderResultsRequestModel() => new SaveBasketFromProviderSearchViewModel
        {
            ItemId = $"{UKPRN.ToString()},{LOCATION_ID_TO_ADD}",
            SearchQuery = new Components.ViewComponents.TrainingProvider.Search.TrainingProviderSearchViewModel
            {
                ApprenticeshipId = APPRENTICESHIP_ID,
                IsLevyPayer = true,
                Keywords = "some keywords",
                Page = 1,
                Postcode = "AB12 3TR",
                ResultsToTake = 10,
                SortOrder = 1
            }
        };

        private static DeleteFromBasketViewModel GetDeleteFromBasketViewModel() => new DeleteFromBasketViewModel()
        {
            ApprenticeshipId = APPRENTICESHIP_ID,
            Ukprn = UKPRN
        };

        private static ApprenticeshipFavouritesBasketRead GetApprenticeshipFavouritesBasketRead()
        {

            var basket = new ApprenticeshipFavouritesBasket();
            basket.Add(APPRENTICESHIP_ID, UKPRN);

            return new ApprenticeshipFavouritesBasketRead(basket);
        }
    }
}
