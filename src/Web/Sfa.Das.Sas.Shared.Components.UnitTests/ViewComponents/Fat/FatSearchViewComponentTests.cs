using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NUnit.Framework;
using Sfa.Das.Sas.Shared.Components.ViewComponents;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat
{
    [TestFixture]
    public class FatSearchViewComponentTests
    {
        private FatSearchViewComponent _sut;


        [SetUp]
        public void Setup()
        {
            var httpContext = new DefaultHttpContext();
            var viewContext = new ViewContext();
            viewContext.HttpContext = httpContext;
            var viewComponentContext = new ViewComponentContext();
            viewComponentContext.ViewContext = viewContext;

            _sut = new FatSearchViewComponent();
            _sut.ViewComponentContext = viewComponentContext;
        }

        [Test]
        public async Task When_Inline_Option_Is_Not_Provided_Then_Return_Default_View()
        {
            var result = await _sut.InvokeAsync("", "") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewName.Should().Be("Default");
        }
        [Test]
        public async Task When_Inline_Option_Is_Provided_And_False_Then_Return_Default_View()
        {
            var result = await _sut.InvokeAsync("", "") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewName.Should().Be("Default");
        }

        [Test]
        public async Task When_Inline_Option_Is_Provided_And_True_Then_Return_Default_View()
        {
            var result = await _sut.InvokeAsync("", "", inline: true) as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewName.Should().Be("Inline");
        }

        [Test]
        public async Task Then_FatSearchViewModel_Is_Returned()
        {
            var keyword = "keyword";

            var result = await _sut.InvokeAsync(keyword, "") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewData.Model.Should().BeOfType<FatSearchViewModel>();
        }

        [Test]
        public async Task When_Keyword_Is_Provided_And_True_Then_Is_Returned_In_Model()
        {
            var keyword = "keyword";

            var result = await _sut.InvokeAsync(keyword, "") as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewData.Model.Should().BeOfType<FatSearchViewModel>();
            var model = result.ViewData.Model as FatSearchViewModel;
            model.Keywords.Should().Be(keyword);
        }
      
    }
}
