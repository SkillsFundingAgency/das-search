using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NUnit.Framework;
using Sfa.Das.Sas.Shared.Components.ViewComponents;
using System.Threading.Tasks;
using Moq;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat
{
    [TestFixture]
    public class FatSearchViewComponentTests : ViewComponentTestsBase
    {
        private FatSearchViewComponent _sut;

        [SetUp]
        public void Setup()
        {
            base.Setup();
            _sut = new FatSearchViewComponent(_cssClasses.Object);
            _sut.ViewComponentContext = _viewComponentContext;
        }

        [Test]
        public async Task When_Inline_Option_Is_Not_Provided_Then_Return_Default_View()
        {
            var result = await _sut.InvokeAsync("") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewName.Should().Be("Default");
        }
        [Test]
        public async Task When_Inline_Option_Is_Provided_And_False_Then_Return_Default_View()
        {
            var result = await _sut.InvokeAsync("", inline: false) as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewName.Should().Be("Default");
        }

        [Test]
        public async Task When_Inline_Option_Is_Provided_And_True_Then_Return_Inline_View()
        {
            var result = await _sut.InvokeAsync("", inline: true) as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewName.Should().Be("Inline");
        }

        [Test]
        public async Task Then_FatSearchViewModel_Is_Returned()
        {
            var keyword = "keyword";

            var result = await _sut.InvokeAsync(keyword) as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewData.Model.Should().BeOfType<FatSearchViewModel>();
        }

        [Test]
        public async Task When_Keyword_Is_Provided_And_True_Then_Is_Returned_In_Model()
        {
            var keyword = "keyword";

            var result = await _sut.InvokeAsync(keyword) as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewData.Model.Should().BeOfType<FatSearchViewModel>();
            var model = result.ViewData.Model as FatSearchViewModel;
            model.Keywords.Should().Be(keyword);
        }
      
    }
}
