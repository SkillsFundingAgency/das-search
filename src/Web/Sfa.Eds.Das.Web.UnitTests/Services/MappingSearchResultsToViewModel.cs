namespace Sfa.Eds.Das.Web.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Sfa.Das.ApplicationServices.Models;

    using ViewModels;
    using Web.Services;

    [TestFixture]
    public class MappingSearchResultsToViewModel
    {
        [Test]
        public void MappSearchResultsToViewModel()
        {
            MappingService mappingService = new MappingService(null);
            var sri = new StandardSearchResultsItem
            {
                StandardId = 101,
                Title = "Standard 1"
            };
            var resultList = new List<StandardSearchResultsItem> { sri };
            var model = new StandardSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList };

            var mappedResult = mappingService.Map<StandardSearchResults, StandardSearchResultViewModel>(model);

            Assert.AreEqual(model.TotalResults, mappedResult.TotalResults);
            Assert.AreEqual(model.Results.First().Title, mappedResult.Results.First().Title);
        }
    }
}
