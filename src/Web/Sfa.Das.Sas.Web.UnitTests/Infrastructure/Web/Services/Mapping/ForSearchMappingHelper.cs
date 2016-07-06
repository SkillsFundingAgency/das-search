namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services.Mapping
{
    using FluentAssertions;

    using NUnit.Framework;

    using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

    [TestFixture]
    public class ForSearchMappingHelper
    {
        [TestCase(100, 10, 10)]
        [TestCase(101, 10, 11)]
        [TestCase(99, 10, 10)]
        [TestCase(89, 10, 9)]
        [TestCase(89, 0, 0, Description = "results to take must me more than 0")]
        [TestCase(-10, 10, -1)]
        public void ShouldCalculateLastPage(long totalResults, int resultsToTake, int expected)
        {
            SearchMappingHelper.CalculateLastPage(totalResults, resultsToTake).Should().Be(expected);
        }
    }
}
