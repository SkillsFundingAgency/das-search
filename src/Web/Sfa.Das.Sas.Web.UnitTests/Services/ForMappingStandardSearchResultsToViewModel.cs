using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Services
{
    using FluentAssertions;

    [TestFixture]
    public sealed class ForMappingStandardSearchResultsToViewModel
    {
        [Test]
        public void WhenTypicalLengthIsEmpty()
        {
            MappingService mappingService = new MappingService(null);
            var sri = new ApprenticeshipSearchResultsItem
            {
                StandardId = 101,
                Title = "Standard 1"
            };
            var resultList = new List<ApprenticeshipSearchResultsItem> { sri };
            var model = new ApprenticeshipSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList, LevelAggregation = new Dictionary<int, long?>() };

            var mappedResult = mappingService.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(model);

            Assert.AreEqual(model.TotalResults, mappedResult.TotalResults);
            Assert.AreEqual(model.Results.First().Title, mappedResult.Results.First().Title);
            Assert.AreEqual(string.Empty, mappedResult.Results.First().TypicalLengthMessage);
        }

        [Test]
        public void WhenTypicalLengthIsRange()
        {
            MappingService mappingService = new MappingService(null);
            var sri = new ApprenticeshipSearchResultsItem
            {
                StandardId = 101,
                Title = "Standard 1",
                TypicalLength = new TypicalLength
                                    {
                                        From = 12,
                                        To = 24,
                                        Unit = "m"
                                    }
            };
            var resultList = new List<ApprenticeshipSearchResultsItem> { sri };
            var model = new ApprenticeshipSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList };

            var mappedResult = mappingService.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(model);

            Assert.AreEqual(model.TotalResults, mappedResult.TotalResults);
            Assert.AreEqual(model.Results.First().Title, mappedResult.Results.First().Title);
            Assert.AreEqual("12 to 24 months", mappedResult.Results.First().TypicalLengthMessage);
        }

        [Test]
        public void WhenTypicalLengthIsFixed()
        {
            MappingService mappingService = new MappingService(null);
            var sri = new ApprenticeshipSearchResultsItem
            {
                StandardId = 101,
                Title = "Standard 1",
                TypicalLength = new TypicalLength
                {
                    From = 24,
                    To = 24,
                    Unit = "m"
                }
            };

            var sri2 = new ApprenticeshipSearchResultsItem
            {
                StandardId = 102,
                Title = "Standard 2",
                TypicalLength = new TypicalLength
                {
                    From = 0,
                    To = 23,
                    Unit = "m"
                }
            };

            var sri3 = new ApprenticeshipSearchResultsItem
            {
                StandardId = 103,
                Title = "Standard 3",
                TypicalLength = new TypicalLength
                {
                    From = 22,
                    To = 0,
                    Unit = "m"
                }
            };

            var resultList = new List<ApprenticeshipSearchResultsItem> { sri, sri2, sri3 };
            var model = new ApprenticeshipSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList };

            var mappedResult = mappingService.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(model);

            Assert.AreEqual(model.TotalResults, mappedResult.TotalResults);
            Assert.AreEqual(model.Results.First().Title, mappedResult.Results.First().Title);
            Assert.AreEqual("24 months", mappedResult.Results.First().TypicalLengthMessage);

            Assert.AreEqual("23 months", mappedResult.Results.First(m => m.Title == "Standard 2").TypicalLengthMessage);

            Assert.AreEqual("22 months", mappedResult.Results.First(m => m.Title == "Standard 3").TypicalLengthMessage);
        }

        [Test]
        public void WhenTypicalLengthIsInvalid()
        {
            MappingService mappingService = new MappingService(null);
            var searchResultItem1 = new ApprenticeshipSearchResultsItem
            {
                StandardId = 101,
                Title = "Standard 1",
                TypicalLength = new TypicalLength
                {
                    From = 24,
                    To = 12,
                    Unit = "m"
                }
            };

            var resultList = new List<ApprenticeshipSearchResultsItem> { searchResultItem1 };
            var model = new ApprenticeshipSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList };

            var mappedResult = mappingService.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(model);

            Assert.AreEqual(string.Empty, mappedResult.Results.First().TypicalLengthMessage);
        }

        [Test]
        public void WhenMappingLevelAggregation()
        {
            MappingService mappingService = new MappingService(null);
            var searchResultItem1 = new ApprenticeshipSearchResultsItem
            {
                StandardId = 101,
                Title = "Standard 1",
                TypicalLength = new TypicalLength
                {
                    From = 24,
                    To = 12,
                    Unit = "m"
                },
            };

            var resultList = new List<ApprenticeshipSearchResultsItem> { searchResultItem1 };
            var levels = new Dictionary<int, long?> { { 2, 3L }, { 3, 38L }, { 4, 380L } };
            var model = new ApprenticeshipSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList, LevelAggregation = levels, SelectedLevels = new List<int> { 2 } };

            var mappedResult = mappingService.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(model);

            mappedResult.AggregationLevel.Count().Should().Be(3);
            mappedResult.AggregationLevel.FirstOrDefault(m => m.Value == "2")?.Checked.Should().BeTrue();
            mappedResult.AggregationLevel.FirstOrDefault(m => m.Value == "3")?.Checked.Should().BeFalse();

            mappedResult.AggregationLevel.FirstOrDefault(m => m.Value == "4")?.Count.Should().Be(380);
        }

        [Test]
        public void WhenMappingLevelAggregationIsNull()
        {
            MappingService mappingService = new MappingService(null);
            var searchResultItem1 = new ApprenticeshipSearchResultsItem
            {
                StandardId = 101,
                Title = "Standard 1",
                TypicalLength = new TypicalLength
                {
                    From = 24,
                    To = 12,
                    Unit = "m"
                },
            };

            var resultList = new List<ApprenticeshipSearchResultsItem> { searchResultItem1 };
            var model = new ApprenticeshipSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList, LevelAggregation = null, SelectedLevels = new List<int> { 2 } };

            var mappedResult = mappingService.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(model);

            mappedResult.AggregationLevel.Count().Should().Be(0);
        }
    }
}
