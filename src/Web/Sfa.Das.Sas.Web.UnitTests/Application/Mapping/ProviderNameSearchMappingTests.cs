using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Mapping
{
    [TestFixture]
    public class ProviderNameSearchMappingTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
                  }

        [TestCase("college", 11112222, "university of life", "Fire Colleges, Air collegiate, college of water", "Fire Colleges,college of water")]
        [TestCase("college", 11112222, "university of life", "Air collegiate , Fire Colleges , college of water  ", "Fire Colleges,college of water")]
        [TestCase("coll", 11112222, "university of life", "Air collegiate,Fire Colleges, college of water", "Air collegiate,Fire Colleges,college of water")]
        [TestCase("uni", 11112222, "university of life", "Air collegiate,Fire Colleges, college of water", "")]
        public void ShouldReturnExpectedMappingsFromSingleCase(string searchTerm, int ukprn, string providername, string preprocessedAliases, string expectedList)
        {
            var preformattedResults = new List<ProviderNameSearchResult>
            {
                new ProviderNameSearchResult { UkPrn = ukprn, ProviderName = providername, Aliases = preprocessedAliases.Split(',').ToList() }
            };

            var expectedResultDetails = expectedList.Trim() == string.Empty ? new List<string>() : expectedList.Split(',').ToList();

            var expectedResults = new List<ProviderNameSearchResult>
            {
                new ProviderNameSearchResult { UkPrn = ukprn, ProviderName = providername, Aliases = expectedResultDetails }
            };

            var res = new ProviderNameSearchMapping().FilterNonMatchingAliases(searchTerm, preformattedResults);
            res.ShouldBeEquivalentTo(expectedResults);
        }

        [Test]
        public void ShouldReturnExpectedMappingsFromGroupsOfItems()
        {
            const string searchTerm = "college";
            var preformattedResults = new List<ProviderNameSearchResult>
            {
                new ProviderNameSearchResult { UkPrn = 11112222, ProviderName = "university of life", Aliases = new List<string> {"Air Uni", "Fire Colleges", "college of water"} },
                new ProviderNameSearchResult { UkPrn = 11112223, ProviderName = "university of fate", Aliases = new List<string> {"college of gold", "Silver Colleges", "school of rock" } }
            };

            var expectedResults = new List<ProviderNameSearchResult>
            {
                new ProviderNameSearchResult { UkPrn = 11112222, ProviderName = "university of life", Aliases = new List<string> {"Fire Colleges", "college of water"} },
                new ProviderNameSearchResult { UkPrn = 11112223, ProviderName = "university of fate", Aliases = new List<string> {"college of gold", "Silver Colleges"} }
            };

            var res = new ProviderNameSearchMapping().FilterNonMatchingAliases(searchTerm, preformattedResults);
            res.ShouldBeEquivalentTo(expectedResults);
        }

    }
}
