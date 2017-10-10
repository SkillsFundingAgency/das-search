using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services
{
    [TestFixture]
    public sealed class ForMappingApprenticeshipSearchResponseToViewModel
    {
        [Test]
        public void WhenMappingLevelAggregation()
        {
            MappingService mappingService = new MappingService(null);
            var searchResultItem1 = new ApprenticeshipSearchResultsItem
            {
                StandardId = "101",
                Title = "Standard 1"
            };

            var resultList = new List<ApprenticeshipSearchResultsItem> { searchResultItem1 };
            var levels = new Dictionary<int, long?> { { 2, 3L }, { 3, 38L }, { 4, 380L } };
            var model = new ApprenticeshipSearchResponse { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList, AggregationLevel = levels, SelectedLevels = new List<int> { 2 } };

            var mappedResult = mappingService.Map<ApprenticeshipSearchResponse, ApprenticeshipSearchResultViewModel>(model);

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
                StandardId = "101",
                Title = "Standard 1"
            };

            var resultList = new List<ApprenticeshipSearchResultsItem> { searchResultItem1 };
            var model = new ApprenticeshipSearchResponse { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList, AggregationLevel = null, SelectedLevels = new List<int> { 2 } };

            var mappedResult = mappingService.Map<ApprenticeshipSearchResponse, ApprenticeshipSearchResultViewModel>(model);

            mappedResult.AggregationLevel.Count().Should().Be(0);
        }
    }
}
