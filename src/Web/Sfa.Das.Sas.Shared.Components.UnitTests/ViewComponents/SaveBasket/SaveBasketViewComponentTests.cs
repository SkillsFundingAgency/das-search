using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Basket;
using System;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat
{
    [TestFixture]
    public class SaveBasketViewComponentTests : ViewComponentTestsBase
    {
        private Mock<ICookieManager> _mockCookieManager;
        private Mock<IFatConfigurationSettings> _mockConfiguration;
        private SaveBasketViewComponent _sut;
        private Guid BasketId;
        private const string EMP_FAV_SAVE_URL = "https://emp-fav/save-url?basketId=";

        [SetUp]
        public new void Setup()
        {
            base.Setup();

            BasketId = Guid.NewGuid();

            _mockCookieManager = new Mock<ICookieManager>();
            _mockCookieManager.Setup(x => x.Get(CookieNames.BasketCookie)).Returns(BasketId.ToString());

            _mockConfiguration = new Mock<IFatConfigurationSettings>();
            _mockConfiguration.SetupGet(x => x.SaveEmployerFavouritesUrl).Returns(EMP_FAV_SAVE_URL);

            _sut = new SaveBasketViewComponent(_mockConfiguration.Object, _mockCookieManager.Object)
            {
                ViewComponentContext = _viewComponentContext
            };
        }

        [Test]
        public void Invoke_ReturnsModelContainingSaveUrl()
        {
            var result = _sut.Invoke() as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<SaveBasketViewModel>();
            var model = result.ViewData.Model as SaveBasketViewModel;
            model.SaveBasketUrl.Should().Be(EMP_FAV_SAVE_URL + BasketId);
        }
    }
}
