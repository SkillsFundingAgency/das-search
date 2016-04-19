namespace Sfa.Eds.Das.Web.UnitTests.Views
{
    using System.Collections.Generic;

    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using ViewModels;
    using Web.Views.Apprenticeship;

    [TestFixture]
    public sealed class ForStandardSearchResultPage : ViewTestBase
    {
        [Test]
        public void WhenSearchResultHasErrors()
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
        public void WhenSearchResultIsZero()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 0,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>(),
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There are no apprenticeships matching your search for 'SearchTerm'");
        }

        [Test]
        public void WhenSearchResultReturnsAll()
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

            GetPartial(html, "p").Should().Be("All apprenticeships.");
        }

        [Test]
        public void WhenSearchResultReturnsOneResult()
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
            var result = GetPartial(html, "p");

            result.Should().Be("There is 1 apprenticeship matching your search for 'SearchTerm'.");
        }

        [Test]
        public void WhenSearchResultReturnsMoreThanOne()
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

            GetPartial(html, "p").Should().Be("There are 2 apprenticeships matching your search for 'SearchTerm'.");
        }

        [Test]
        [Ignore]
        public void When_SearchResultContainLevel()
        {
            var searchPage = new SearchResults();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 2,
                SearchTerm = "test",
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months"
                                      },
                                  new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 2",
                                        Level = "3 (equivalent to 2 A level passes)"
                                      }
                              }
            };
            var html = searchPage.RenderAsHtml(model).ToAngleSharp();

            // First result
            GetPartialWhere(html, ".result", "Test title 1").Should().Contain("72 months");
            GetPartialWhere(html, ".result", "Test title 1").Should().Contain("Typical length:");

            // Second result
            GetPartialWhere(html, ".result", "Test title 2").Should().NotContain("72 months");
            GetPartialWhere(html, ".result", "Test title 2").Should().NotContain("Typical length:");
            GetPartialWhere(html, ".result", "Test title 2").Should().Contain("Level");
            GetPartialWhere(html, ".result", "Test title 2").Should().Contain("3 (equivalent to 2 A level passes)");
        }
    }
}
