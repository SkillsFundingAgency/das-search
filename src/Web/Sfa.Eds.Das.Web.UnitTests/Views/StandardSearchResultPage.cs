namespace Sfa.Eds.Das.Web.UnitTests.Views
{
    using System.Collections.Generic;

    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using ViewModels;
    using Web.Views.Standard;

    [TestFixture]
    public sealed class StandardSearchResultPage : ViewTestBase
    {
        [Test]
        public void When_SearchResultHasErrors()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 0,
                SearchTerm = string.Empty,
                Results = new List<ApprenticeshipSearchResultItemViewModel>(),
                HasError = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There was a problem performing a search. Try again later.");
        }

        [Test]
        public void When_SearchResultIsZero()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 0,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>(),
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There are no standards matching your search for 'SearchTerm'");
        }

        [Test]
        public void When_SearchResultReturnsAll()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 68,
                SearchTerm = string.Empty,
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "h2").Should().Be("Apprenticeship standards");
            GetPartial(html, "p").Should().Be("All apprenticeship standards.");
        }

        [Test]
        public void When_SearchResultReturnsOneResult()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 1,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            var resultHeading = GetPartial(html, "h2");
            var result = GetPartial(html, "p");

            resultHeading.Should().Be("Apprenticeship standards");
            result.Should().Be("There is 1 standard matching your search for 'SearchTerm'.");
        }

        [Test]
        public void When_SearchResultReturnsMoreThanOne()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 2,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "h2").Should().Be("Apprenticeship standards");
            GetPartial(html, "p").Should().Be("There are 2 standards matching your search for 'SearchTerm'.");
        }
    }
}
