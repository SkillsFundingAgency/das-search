using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Basket.Models;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Basket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat
{
    [TestFixture]
    public class AddToBasketViewComponentTests : ViewComponentTestsBase
    {
        private Mock<ICookieManager> _mockCookieManager;
        private Mock<IMediator> _mockMediator;
        private AddToBasketViewComponent _sut;

        [SetUp]
        public new void Setup()
        {
            base.Setup();

            var cookieBasketId = Guid.NewGuid();

            _mockCookieManager = new Mock<ICookieManager>();
            _mockCookieManager.Setup(x => x.Get(CookieNames.BasketCookie)).Returns(cookieBasketId.ToString());

            _mockMediator = new Mock<IMediator>();
            _mockMediator.Setup(x => x.Send(It.Is<GetBasketQuery>(a => a.BasketId == cookieBasketId), default(CancellationToken)))
                .ReturnsAsync(new ApprenticeshipFavouritesBasket { new ApprenticeshipFavourite { ApprenticeshipId = "420-2-1" } });

            _sut = new AddToBasketViewComponent(_mockMediator.Object, _mockCookieManager.Object)
            {
                ViewComponentContext = _viewComponentContext
            };
        }

        [Test]
        public async Task Invoke_ReturnsModelContainingApprenticeshipId()
        {
            var result = await _sut.InvokeAsync("100") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<AddToBasketViewModel>();
            (result.ViewData.Model as AddToBasketViewModel).ApprenticeshipId.Should().Be("100");
        }

        [Test]
        public async Task Invoke_ReturnsModelContainingIndicatorTrue_IfApprenticehshipAlreadyInBasket()
        {
            var result = await _sut.InvokeAsync("420-2-1") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<AddToBasketViewModel>();
            (result.ViewData.Model as AddToBasketViewModel).IsInBasket.Should().BeTrue();
        }

        [Test]
        public async Task Invoke_ReturnsModelContainingIndicatorFalse_IfApprenticehshipNotAlreadyInBasket()
        {
            var result = await _sut.InvokeAsync("555-2-1") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<AddToBasketViewModel>();
            (result.ViewData.Model as AddToBasketViewModel).IsInBasket.Should().BeFalse();
        }

        [Test]
        public async Task Invoke_ReturnsDefaultView()
        {
            var result = await _sut.InvokeAsync("420-2-1") as ViewViewComponentResult;

            result.ViewName.Should().Be("Default");
        }
    }
}
