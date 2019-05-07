using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NUnit.Framework;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Basket.SaveApprenticeship;
using Sfa.Das.Sas.Shared.Components.ViewComponents.SaveApprenticeship;
using Sfa.Das.Sas.Shared.Components.Configuration;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat
{
    [TestFixture]
    public class SaveApprenticeshipComponentTests : ViewComponentTestsBase
    {
        private SaveApprenticeshipViewComponent _sut;

        [SetUp]
        public new void Setup()
        {
            base.Setup();

            var config = new FatSharedComponentsConfiguration { EmployerFavouritesUrl = "http://employer-favourites" };

            _sut = new SaveApprenticeshipViewComponent(config)
            {
                ViewComponentContext = _viewComponentContext
            };
        }

        [Test]
        public void Invoke_ReturnsModelContainingUrlToEmployerFavourties_ForStandard()
        {
            var result = _sut.Invoke("100") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<SaveApprenticeshipViewModel>();
            (result.ViewData.Model as SaveApprenticeshipViewModel).SaveUrl.Should().Be("http://employer-favourites/save-apprenticeship-favourites?apprenticeshipId=100");
        }

        [Test]
        public void Invoke_ReturnsModelContainingUrlToEmployerFavourties_ForFramework()
        {
            var result = _sut.Invoke("420-2-1") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();
            result.ViewData.Model.Should().BeAssignableTo<SaveApprenticeshipViewModel>();
            (result.ViewData.Model as SaveApprenticeshipViewModel).SaveUrl.Should().Be("http://employer-favourites/save-apprenticeship-favourites?apprenticeshipId=420-2-1");
        }

        [Test]
        public void Invoke_ReturnsDefaultView()
        {
            var result = _sut.Invoke("420-2-1") as ViewViewComponentResult;

            result.ViewName.Should().Be("Default");
        }
    }
}
