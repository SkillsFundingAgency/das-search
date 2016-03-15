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
    public sealed class ForStandardSearchResultPage : ViewTestBase
    {
        [Test]
        public void WhenSearchResultHasErrors()
        {
            var detail = new SearchResultMessage();
            var model = new StandardSearchResultViewModel
            {
                TotalResults = 0,
                SearchTerm = string.Empty,
                Results = new List<StandardSearchResultItemViewModel>(),
                HasError = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There was a problem performing a search. Try again later.");
        }

        [Test]
        public void WhenSearchResultIsZero()
        {
            var detail = new SearchResultMessage();
            var model = new StandardSearchResultViewModel
            {
                TotalResults = 0,
                SearchTerm = "SearchTerm",
                Results = new List<StandardSearchResultItemViewModel>(),
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There are no standards matching your search for 'SearchTerm'");
        }

        [Test]
        public void WhenSearchResultReturnsAll()
        {
            var detail = new SearchResultMessage();
            var model = new StandardSearchResultViewModel
            {
                TotalResults = 68,
                SearchTerm = string.Empty,
                Results = new List<StandardSearchResultItemViewModel>
                              {
                                  new StandardSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "h2").Should().Be("Apprenticeship standards");
            GetPartial(html, "p").Should().Be("All apprenticeship standards.");
        }

        [Test]
        public void WhenSearchResultReturnsOneResult()
        {
            var detail = new SearchResultMessage();
            var model = new StandardSearchResultViewModel
            {
                TotalResults = 1,
                SearchTerm = "SearchTerm",
                Results = new List<StandardSearchResultItemViewModel>
                              {
                                  new StandardSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            var resultHeading = GetPartial(html, "h2");
            var result = GetPartial(html, "p");

            resultHeading.Should().Be("Apprenticeship standards");
            result.Should().Be("There is 1 standard matching your search for 'SearchTerm'.");
        }

        [Test]
        public void WhenSearchResultReturnsMoreThanOne()
        {
            var detail = new SearchResultMessage();
            var model = new StandardSearchResultViewModel
            {
                TotalResults = 2,
                SearchTerm = "SearchTerm",
                Results = new List<StandardSearchResultItemViewModel>
                              {
                                  new StandardSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "h2").Should().Be("Apprenticeship standards");
            GetPartial(html, "p").Should().Be("There are 2 standards matching your search for 'SearchTerm'.");
        }
    }
}
