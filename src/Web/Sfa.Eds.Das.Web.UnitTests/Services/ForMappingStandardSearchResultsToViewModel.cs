namespace Sfa.Eds.Das.Web.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Domain.Model;
    using NUnit.Framework;
    using Sfa.Das.ApplicationServices.Models;
    using ViewModels;
    using Web.Services;

    [TestFixture]
    public sealed class ForMappingStandardSearchResultsToViewModel
    {
        [Test]
        public void WhenTypicalLengthIsEmpty()
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
            Assert.AreEqual(string.Empty, mappedResult.Results.First().TypicalLengthMessage);
        }

        [Test]
        public void WhenTypicalLengthIsRange()
        {
            MappingService mappingService = new MappingService(null);
            var sri = new StandardSearchResultsItem
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
            var resultList = new List<StandardSearchResultsItem> { sri };
            var model = new StandardSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList };

            var mappedResult = mappingService.Map<StandardSearchResults, StandardSearchResultViewModel>(model);

            Assert.AreEqual(model.TotalResults, mappedResult.TotalResults);
            Assert.AreEqual(model.Results.First().Title, mappedResult.Results.First().Title);
            Assert.AreEqual("12 to 24 months", mappedResult.Results.First().TypicalLengthMessage);
        }

        [Test]
        public void WhenTypicalLengthIsFixed()
        {
            MappingService mappingService = new MappingService(null);
            var sri = new StandardSearchResultsItem
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

            var sri2 = new StandardSearchResultsItem
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

            var sri3 = new StandardSearchResultsItem
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

            var resultList = new List<StandardSearchResultsItem> { sri, sri2, sri3 };
            var model = new StandardSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList };

            var mappedResult = mappingService.Map<StandardSearchResults, StandardSearchResultViewModel>(model);

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
            var searchResultItem1 = new StandardSearchResultsItem
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

            var resultList = new List<StandardSearchResultsItem> { searchResultItem1 };
            var model = new StandardSearchResults { TotalResults = 1234L, SearchTerm = "apprenticeship", Results = resultList };

            var mappedResult = mappingService.Map<StandardSearchResults, StandardSearchResultViewModel>(model);

            Assert.AreEqual(string.Empty, mappedResult.Results.First().TypicalLengthMessage);
        }
    }
}
