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

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Controller
{
    [TestFixture]
    public class BasketControllerTests
    {
        private const string APPRENTICESHIP_ID = "123";
        private const int UKPRN = 12345678;
        private const int LOCATION_ID = 123123;
        private const string BasketCookieName = "ApprenticeshipBasket";
        private Mock<IMediator> _mockMediator;
        private Mock<ICookieManager> _mockCookieManager;
        private BasketController _sut;

        private SaveBasketFromApprenticeshipDetailsViewModel _addFromApprenticeshipDetailsModel = new SaveBasketFromApprenticeshipDetailsViewModel
        {
            ItemId = APPRENTICESHIP_ID
        };

        private SaveBasketFromApprenticeshipResultsViewModel _addFromApprenticeshipResultsModel = new SaveBasketFromApprenticeshipResultsViewModel
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

        private SaveBasketFromProviderDetailsViewModel _addFromProviderDetailsModel = new SaveBasketFromProviderDetailsViewModel
        {
            ItemId = UKPRN,
            ApprenticeshipId = APPRENTICESHIP_ID,
            ApprenticeshipType = ApplicationServices.Models.ApprenticeshipType.Standard,
            LocationId = LOCATION_ID,
            Page = 1,
            Ukprn = UKPRN,
            ViewType = ViewModels.ViewType.Details
        };

        private SaveBasketFromProviderSearchViewModel _addFromProviderSearchModel = new SaveBasketFromProviderSearchViewModel
        {
            ItemId = UKPRN,
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


        [SetUp]
        public void Setup()
        {
            // Set cookie in http request
            _mockMediator = new Mock<IMediator>();
            _mockCookieManager = new Mock<ICookieManager>();

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

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.ApprenticeshipId == "123"), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());

            var result = await _sut.AddApprenticeshipFromDetails(_addFromApprenticeshipDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddApprenticeshipFromDetails(_addFromApprenticeshipDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
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

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.ApprenticeshipId == "123"), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromResults_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());

            var result = await _sut.AddApprenticeshipFromResults(_addFromApprenticeshipResultsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromResults_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddApprenticeshipFromResults(_addFromApprenticeshipResultsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromResults_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
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

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.ApprenticeshipId == APPRENTICESHIP_ID && a.Ukprn == UKPRN), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromDetails_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());

            var result = await _sut.AddProviderFromDetails(_addFromProviderDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromDetails_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddProviderFromDetails(_addFromProviderDetailsModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromDetails_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
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
            routeValues["DeliveryModes"].Should().Be(_addFromProviderSearchModel.SearchQuery.DeliveryModes);
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

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.ApprenticeshipId == APPRENTICESHIP_ID && a.Ukprn == UKPRN), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromResults_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());

            var result = await _sut.AddProviderFromResults(_addFromProviderSearchModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromResults_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddProviderFromResults(_addFromProviderSearchModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddProviderFromResults_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
            _mockCookieManager.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>()));

            var result = await _sut.AddProviderFromResults(_addFromProviderSearchModel);

            _mockCookieManager.Verify(x => x.Set(BasketCookieName, newBasketId.ToString(), It.IsAny<DateTimeOffset?>()));
        }

        #endregion
    }
}
