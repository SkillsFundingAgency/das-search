using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.Shared.Components.Controllers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Controller
{
    [TestFixture]
    public class BasketControllerTests
    {
        private const string APPRENTICESHIP_SELECTED_ITEM = "123$";
        private const string APPRENTICESHIP_WITH_UKPRN_SELECTED_ITEM = "123$50";
        private const string BasketCookieName = "ApprenticeshipBasket";
        private Mock<IMediator> _mockMediator;
        private BasketController _sut;
        private Mock<HttpContext> _mockContext;
        private Dictionary<string, string> _cookies;
        
        [SetUp]
        public void Setup()
        {
            // Set cookie in http request
            _cookies = new Dictionary<string, string>();
            _mockContext = GetMockHttpContext(_cookies);

            _mockMediator = new Mock<IMediator>();
            _sut = new BasketController(_mockMediator.Object);
            _sut.ControllerContext.HttpContext = _mockContext.Object;
        }

        //TODO: LWA - Need to redirect back to origin of request.
        [Test]
        public async Task Add_ReturnsRedirectResult()
        {
            var result = await _sut.Add(APPRENTICESHIP_SELECTED_ITEM);

            result.Should().BeAssignableTo<RedirectResult>();
        }

        [Test]
        public async Task Add_ParsesApprenticeshipId_FromArgument()
        {
            var result = await _sut.Add(APPRENTICESHIP_SELECTED_ITEM);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.ApprenticeshipId == "123"), default(CancellationToken)));
        }

        //TODO: Add in when introduce selecting training provider
        //[Test]
        //public async Task Add_ParsesApprenticeshipIdAndUkprn_FromArgument()
        //{
        //    var result = await _sut.Add(APPRENTICESHIP_WITH_UKPRN_SELECTED_ITEM);

        //    _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.ApprenticeshipId == "123" && a.Ukprn == 50), default(CancellationToken)));
        //}

        [Test]
        public async Task Add_UsesBasketIdFromCookie_IfCookieExists()
        {
            var BasketIdFromCookie = Guid.NewGuid();
            _cookies.Add(BasketCookieName, BasketIdFromCookie.ToString());

            var result = await _sut.Add(APPRENTICESHIP_SELECTED_ITEM);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == BasketIdFromCookie), default(CancellationToken)));
        }

        [Test]
        public async Task Add_UsesNullForBasketId_IfNoCookieExists()
        {
            var result = await _sut.Add(APPRENTICESHIP_SELECTED_ITEM);

            _mockMediator.Verify(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken)));
        }

        [Test]
        public async Task Add_SavesBasketIdToCookie()
        {
            var newBasketId = Guid.NewGuid(); // Setup basket it to be returned by save logic
            _mockMediator.Setup(x => x.Send(It.Is<AddFavouriteToBasketCommand>(a => a.BasketId == null), default(CancellationToken))).ReturnsAsync(newBasketId);
            var cookiesMock = Mock.Get(_mockContext.Object.Response.Cookies); // Setup mock for cookie collection
            cookiesMock.Setup(x => x.Append(It.IsAny<string>(), It.IsAny<string>()));

            var result = await _sut.Add(APPRENTICESHIP_SELECTED_ITEM);

            cookiesMock.Verify(x => x.Append(BasketCookieName, newBasketId.ToString()));
        }

        private static Mock<HttpContext> GetMockHttpContext(Dictionary<string, string> cookies)
        {
            RequestCookieCollection collection = new RequestCookieCollection(cookies);

            var request = new Mock<HttpRequest>();
            request.SetupGet(f => f.Cookies).Returns(collection);

            Mock<HttpContext> mockContext = new Mock<HttpContext> { DefaultValue = DefaultValue.Mock };
            mockContext.SetupGet(x => x.Request).Returns(request.Object);

            return mockContext;
        }
    }
}
