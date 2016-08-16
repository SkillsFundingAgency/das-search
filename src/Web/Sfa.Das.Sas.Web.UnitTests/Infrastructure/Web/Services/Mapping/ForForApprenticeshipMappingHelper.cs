namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services.Mapping
{
    using FluentAssertions;

    using NUnit.Framework;

    using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

    [TestFixture]
    public class ForForApprenticeshipMappingHelper
    {
        [TestCase("", "")]
        [TestCase("Abba", "Abba")]
        [TestCase("Abba:Abba", "Abba")]
        [TestCase("Abba: Abba", "Abba")]
        [TestCase("Abba:Sabba", "Abba:Sabba")]
        [TestCase("Abba: Sabba", "Abba: Sabba")]
        [TestCase("Abba: ", "Abba")]
        public void WhenMappingFrameworkTitle(string input, string expected)
        {
            ApprenticeshipMappingHelper.FrameworkTitle(input).Should().BeEquivalentTo(expected);
        }
    }
}
