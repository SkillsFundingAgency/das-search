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
    public sealed class ForApprenticeshipSearchResultPage : ViewTestBase
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

            GetPartial(html, ".results-error").Should().Contain("There was a problem performing a search. Try again later.");
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

            GetPartial(html, ".results-empty").Should().Contain("There are no apprenticeships matching your search for 'SearchTerm'");
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

            GetPartial(html, ".results-all").Should().Be("All apprenticeships.");
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
            var result = GetPartial(html, ".results-one");

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

            GetPartial(html, ".results-several").Should().Be("There are 2 apprenticeships matching your search for 'SearchTerm'.");
        }

        [Test]
        public void WhenSearchResultReturnsSomethingShouldShowLabelMessage()
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

            GetPartial(html, ".labelmessage").Should().Be("Apprenticeships labelled new are job specific standards, developed by groups of employers.");
        }

        [Test]
        public void WhenSearchResultDoesntReturnsResultsShouldntShowLabelMessage()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 0,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".labelmessage").Should().BeEmpty();
        }

        [Test]
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
            GetPartial(html, ".result-title").Should().Be("Test title 1");
            GetPartial(html, ".length").Should().Be("72 months");

            // Second result
            GetPartial(html, ".result-title", 2).Should().Be("Test title 2");
            GetPartial(html, ".level").Should().Be("3 (equivalent to 2 A level passes)");
        }

        [Test]
        public void WhenSearchResultReturnsAnStandardShouldNewLabelMessage()
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
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                                  new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 2",
                                        Level = "3 (equivalent to 2 A level passes)",
                                        FrameworkId = 3
                                      }
                              }
            };
            var html = searchPage.RenderAsHtml(model).ToAngleSharp();

            // First result
            GetPartial(html, ".result-title").Should().Be("Test title 1 new");

            // Second result
            GetPartial(html, ".result-title", 2).Should().Be("Test title 2");

        }
    }
}
