namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services.Mapping
{
    using FluentAssertions;

    using NUnit.Framework;

    using Sas.Web.Services.MappingActions.Helpers;

    [TestFixture]
    public class ForApprenticeshipMappingHelper
    {
        [TestCase("", "")]
        [TestCase("Abba", "Abba")]
        [TestCase("Abba:Abba", "Abba")]
        [TestCase("Abba: Abba", "Abba")]
        [TestCase("Abba:Sabba", "Abba:Sabba")]
        [TestCase("Abba: Sabba", "Abba: Sabba")]
        [TestCase("Abba: ", "Abba")]
        [TestCase(null, "")]
        public void WhenMappingFrameworkTitle(string input, string expected)
        {
            ApprenticeshipMappingHelper.FrameworkTitle(input).Should().BeEquivalentTo(expected);
        }
    }
}
