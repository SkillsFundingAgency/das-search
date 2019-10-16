using System;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Basket.Models;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Basket;
using Sfa.Das.Sas.ApplicationServices.Models;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Basket
{
    [TestFixture]
    public class BasketIconViewComponentTests : ViewComponentTestsBase
    {
        private const string ExpectedBasketViewUrl = "url-of-basket-view";
        private Mock<ICookieManager> _mockCookieManager;
        private Mock<IMediator> _mockMediator;
        private BasketIconViewComponent _sut;
        private ApprenticeshipFavouritesBasket _basket;
        private Guid _cookieBasketId = Guid.NewGuid();

        [SetUp]
        public new void Setup()
        {
            base.Setup();

            _mockCookieManager = new Mock<ICookieManager>();
            _mockCookieManager.Setup(x => x.Get(CookieNames.BasketCookie)).Returns(_cookieBasketId.ToString());

            _basket = new ApprenticeshipFavouritesBasket();

            _mockMediator = new Mock<IMediator>();

            _sut = new BasketIconViewComponent(_mockMediator.Object, _mockCookieManager.Object)
            {
                ViewComponentContext = _viewComponentContext
            };

            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<object>())).Returns(ExpectedBasketViewUrl);
            _sut.Url = urlHelper.Object;
        }

        [Test]
        public async Task Invoke_ReturnsModelContainingItemCountOfZero_WhenBasketIsEmpty()
        {
            _mockMediator.Setup(x => x.Send(It.Is<GetBasketQuery>(a => a.BasketId == _cookieBasketId), default))
                .ReturnsAsync(new ApprenticeshipFavouritesBasketRead(_basket));

            var result = await _sut.InvokeAsync() as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<BasketIconViewModel>();
            var model = result.ViewData.Model as BasketIconViewModel;
            model.ItemCount.Should().Be(0);
        }

        [Test]
        public async Task Invoke_ReturnsModelContainingItemCountOfZero_WhenNoCookieExists()
        {
            _mockCookieManager.Setup(x => x.Get(CookieNames.BasketCookie)).Returns((string)null);

            var result = await _sut.InvokeAsync() as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<BasketIconViewModel>();
            var model = result.ViewData.Model as BasketIconViewModel;
            model.ItemCount.Should().Be(0);
        }

        [Test]
        public async Task Invoke_ReturnsModelContainingItemCountInBasket_WhenBasketNotEmpty()
        {
            _basket.Add("123");
            _basket.Add("456");
            _mockMediator.Setup(x => x.Send(It.Is<GetBasketQuery>(a => a.BasketId == _cookieBasketId), default))
              .ReturnsAsync(new ApprenticeshipFavouritesBasketRead(_basket));

            var result = await _sut.InvokeAsync() as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<BasketIconViewModel>();
            var model = result.ViewData.Model as BasketIconViewModel;
            model.ItemCount.Should().Be(2);
        }

        [Test]
        public async Task Invoke_ReturnsModelContainingUrlOfBasketPage()
        {
            var result = await _sut.InvokeAsync() as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<BasketIconViewModel>();
            var model = result.ViewData.Model as BasketIconViewModel;
            model.BasketUrl.Should().Be(ExpectedBasketViewUrl);
        }

        [Test]
        public async Task Invoke_ReturnsDefaultView()
        {
            var result = await _sut.InvokeAsync() as ViewViewComponentResult;

            result.ViewName.Should().Be("../Basket/BasketIcon/Default");
        }
    }
}
