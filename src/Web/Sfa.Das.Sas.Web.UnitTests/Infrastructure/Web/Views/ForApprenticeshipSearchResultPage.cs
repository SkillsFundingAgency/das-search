using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Apprenticeship;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views
{
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

            GetPartial(html, ".results-empty").Should().Contain("Sorry, there is currently no apprenticeship training for 'SearchTerm'");
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

            result.Should().Be("1 apprenticeship matching your search for 'SearchTerm'.");
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

            GetPartial(html, ".results-several").Should().Be("2 apprenticeships matching your search for 'SearchTerm'.");
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

            GetPartial(html, ".labelmessage").Should().Be("Results labelled New are apprenticeship standards developed by employer groups.");
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
                              },
                AggregationLevel = new List<LevelAggregationViewModel>(),
                ShortlistedFrameworks = new Dictionary<int, bool>(),
                ShortlistedStandards = new Dictionary<int, bool>()
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
                              },
                AggregationLevel = new List<LevelAggregationViewModel>(),
                ShortlistedFrameworks = new Dictionary<int, bool>(),
                ShortlistedStandards = new Dictionary<int, bool>()
            };
            var html = searchPage.RenderAsHtml(model).ToAngleSharp();

            // First result
            GetPartial(html, ".result-title").Should().StartWith("Test title 1");

            // Second result
            GetPartial(html, ".result-title", 2).Should().Be("Test title 2");
        }

        [Test]
        public void WhenSearchResultHasPaginationAndIsTheFirstPageShouldShowOnlyNextPageLink()
        {
            var detail = new SearchResults();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 20,
                SearchTerm = "*",
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
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      }
                },
                ActualPage = 1,
                LastPage = 2,
                ResultsToTake = 10,
                HasError = false,
                AggregationLevel = new List<LevelAggregationViewModel>(),
                ShortlistedFrameworks = new Dictionary<int, bool>(),
                ShortlistedStandards = new Dictionary<int, bool>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".page-navigation__btn.prev").Should().BeEmpty();
            GetPartial(html, ".page-navigation__btn.next").Should().Contain("Next page").And.Contain("2 of 2");
        }

        [Test]
        public void WhenSearchResultHasPaginationAndIsAPageInTheMiddleShouldShowBackAndNextPageLink()
        {
            var detail = new SearchResults();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 30,
                SearchTerm = "*",
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
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      }
                },
                ActualPage = 2,
                LastPage = 3,
                ResultsToTake = 10,
                HasError = false,
                AggregationLevel = new List<LevelAggregationViewModel>(),
                ShortlistedFrameworks = new Dictionary<int, bool>(),
                ShortlistedStandards = new Dictionary<int, bool>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".page-navigation__btn.prev").Should().Contain("Previous page").And.Contain("1 of 3");
            GetPartial(html, ".page-navigation__btn.next").Should().Contain("Next page").And.Contain("3 of 3");
        }

        [Test]
        public void WhenSearchResultHasPaginationAndIsTheLastPageShouldShowOnlyBackPageLink()
        {
            var detail = new SearchResults();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 30,
                SearchTerm = "*",
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
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      },
                    new ApprenticeshipSearchResultItemViewModel
                                      {
                                        Title = "Test title 1",
                                        TypicalLengthMessage = "72 months",
                                        StandardId = 3
                                      }
                },
                ActualPage = 3,
                LastPage = 3,
                ResultsToTake = 10,
                HasError = false,
                AggregationLevel = new List<LevelAggregationViewModel>(),
                ShortlistedFrameworks = new Dictionary<int, bool>(),
                ShortlistedStandards = new Dictionary<int, bool>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".page-navigation__btn.prev").Should().Contain("Previous page").And.Contain("2 of 3");
            GetPartial(html, ".page-navigation__btn.next").Should().BeEmpty();
        }

        [Test]
        public void WhenLevelAggregationValuesAreSelected()
        {
            var searchPage = new SearchResults();
            var aggList = new List<LevelAggregationViewModel> { new LevelAggregationViewModel { Checked = false, Count = 36, Value = "1" }, new LevelAggregationViewModel { Checked = true, Count = 500, Value = "2" } };
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 1,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel { Title = "Test" }
                              },
                AggregationLevel = aggList,
                ShortlistedFrameworks = new Dictionary<int, bool>(),
                ShortlistedStandards = new Dictionary<int, bool>()
            };

            var html = searchPage.RenderAsHtml(model).ToAngleSharp();

            GetAttribute(html, ".column-one-third form ul li input", "type").Should().Be("checkbox");
            GetAttribute(html, ".column-one-third form ul li input", "checked").Should().BeNull();
            GetAttribute(html, ".column-one-third form ul li input", "checked", 2).Should().Be(string.Empty);
            GetPartial(html, ".column-one-third form ul li label", 2).Should().Be("level 2 (500)");
        }

        [Test]
        public void ShouldShowLinkToSearchAgain()
        {
            var searchPage = new SearchResults();
            var aggList = new List<LevelAggregationViewModel> { new LevelAggregationViewModel { Checked = false, Count = 36, Value = "1" }, new LevelAggregationViewModel { Checked = true, Count = 500, Value = "2" } };
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 1,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel { Title = "Test" }
                              },
                AggregationLevel = aggList,
                ShortlistedFrameworks = new Dictionary<int, bool>(),
                ShortlistedStandards = new Dictionary<int, bool>()
            };

            var html = searchPage.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".new-apprenticeship-search").Should().NotBeNullOrEmpty();
            GetPartial(html, ".new-apprenticeship-search").Should().Be("Start a new search");
        }
    }
}
