namespace Sfa.Eds.Das.Web.UnitTests.Views
{
    using System.Collections.Generic;

    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using ViewModels;
    using Web.Views.Provider;

    [TestFixture]
    public sealed class ProviderSearchResultPage : ViewTestBase
    {
        [Test]
        public void ShouldShowAnErrorWhenSomethingIsWrong()
        {
            var detail = new SearchResultMessage();
            var model = new ProviderSearchResultViewModel
            {
                TotalResults = 0,
                PostCodeMissing = false,
                StandardId = 1,
                Results = new List<ProviderResultItemViewModel>(),
                HasError = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There was a problem performing a search. Try again later.");
        }

        [Test]
        public void ShouldShowIndividualMessageWhenJustOneResultIsReturned()
        {
            var detail = new SearchResultMessage();
            var model = new ProviderSearchResultViewModel
            {
                TotalResults = 1,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Results = new List<ProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There is 1 training provider for the apprenticeship standard: Test name.");
        }

        [Test]
        public void ShouldShowGeneralMessageWhenSeveralResultsAreReturned()
        {
            var detail = new SearchResultMessage();
            var model = new ProviderSearchResultViewModel
            {
                TotalResults = 7,
                PostCodeMissing = false,
                StandardId = 1,
                StandardName = "Test name",
                Results = new List<ProviderResultItemViewModel>(),
                HasError = false
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There are 7 training providers for the apprenticeship standard: Test name.");
        }
    }
}
