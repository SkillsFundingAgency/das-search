using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.Shared.Components.Controllers;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.ViewModels.Basket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Controller
{
    [TestFixture]
    public class BasketControllerTests
    {
        private const string APPRENTICESHIP_ID = "123";
        private const string BasketCookieName = "ApprenticeshipBasket";
        private Mock<IMediator> _mockMediator;
        private Mock<ICookieManager> _mockCookieManager;
        private BasketController _sut;
        private SaveBasketFromApprenticeshipResultsViewModel _searchResultsPageModel = new SaveBasketFromApprenticeshipResultsViewModel
        {
            ApprenticeshipId = APPRENTICESHIP_ID,
            SearchQuery = new ViewModels.SearchQueryViewModel
            {
                Keywords = "baker",
                Page = 3,
                ResultsToTake = 40,
                SortOrder = 1
            }
        };

        [SetUp]
        public void Setup()
        {
            // Set cookie in http request
            _mockMediator = new Mock<IMediator>();
            _mockCookieManager = new Mock<ICookieManager>();

            _sut = new BasketController(_mockMediator.Object, _mockCookieManager.Object);
        }

        #region AddApprenticeshipFromDetails

        [Test]
        public async Task AddApprenticeshipFromDetails_ReturnsRedirectResult_ToApprenticeshipDetailsPage()
        {
            var result = await _sut.AddApprenticeshipFromDetails(APPRENTICESHIP_ID);

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
            var result = await _sut.AddApprenticeshipFromDetails(APPRENTICESHIP_ID);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.ApprenticeshipId == "123"), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());

            var result = await _sut.AddApprenticeshipFromDetails(APPRENTICESHIP_ID);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddApprenticeshipFromDetails(APPRENTICESHIP_ID);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromDetails_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
            _mockCookieManager.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<string>()));

            var result = await _sut.AddApprenticeshipFromDetails(APPRENTICESHIP_ID);

            _mockCookieManager.Verify(x => x.Set(BasketCookieName, newBasketId.ToString()));
        }

        #endregion

        #region AddApprenticeshipFromResults

        [Test]
        public async Task AddApprenticeshipFromResults_ReturnsRedirectResult_ToApprenticeshipDetailsPage()
        {
            var result = await _sut.AddApprenticeshipFromResults(_searchResultsPageModel);

            result.Should().BeAssignableTo<RedirectToActionResult>();
            var redirect = (RedirectToActionResult)result;

            redirect.ControllerName.Should().Be("Fat");
            redirect.ActionName.Should().Be("Search");
            var routeValues = redirect.RouteValues;
            routeValues["Keywords"].Should().Be(_searchResultsPageModel.SearchQuery.Keywords);
            routeValues["Page"].Should().Be(_searchResultsPageModel.SearchQuery.Page);
            routeValues["ResultsToTake"].Should().Be(_searchResultsPageModel.SearchQuery.ResultsToTake);
            routeValues["SortOrder"].Should().Be(_searchResultsPageModel.SearchQuery.SortOrder);
        }

        [Test]
        public async Task AddApprenticeshipFromResults_ParsesApprenticeshipId_FromArgument()
        {
            var result = await _sut.AddApprenticeshipFromResults(_searchResultsPageModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.ApprenticeshipId == "123"), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromResults_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _mockCookieManager.Setup(x => x.Get(BasketCookieName)).Returns(BasketIdFromCookie.ToString());

            var result = await _sut.AddApprenticeshipFromResults(_searchResultsPageModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromResults_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.AddApprenticeshipFromResults(_searchResultsPageModel);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task AddApprenticeshipFromResults_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
            _mockCookieManager.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<string>()));

            var result = await _sut.AddApprenticeshipFromResults(_searchResultsPageModel);

            _mockCookieManager.Verify(x => x.Set(BasketCookieName, newBasketId.ToString()));
        }

        #endregion
    }
}
