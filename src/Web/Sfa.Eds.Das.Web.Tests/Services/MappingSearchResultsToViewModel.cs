namespace Sfa.Eds.Das.Web.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Core.Models;

    using NUnit.Framework;

    using ViewModels;
    using Web.Services;

    [TestFixture]
    public class MappingSearchResultsToViewModel
    {
        [Test]
        public void MappSearchResultsToViewModel()
        {
            MappingService mappingService = new MappingService(null);
            var sri = new SearchResultsItem
            {
                StandardId = 101,
                Title = "Standard 1"
            };
            var resultList = new List<SearchResultsItem> { sri };
            var model = new SearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList };

            var mappedResult = mappingService.Map<SearchResults, StandardSearchResultViewModel>(model);

            Assert.AreEqual(model.TotalResults, mappedResult.TotalResults);
            Assert.AreEqual(model.Results.First().Title, mappedResult.Results.First().Title);
        }
    }
}
