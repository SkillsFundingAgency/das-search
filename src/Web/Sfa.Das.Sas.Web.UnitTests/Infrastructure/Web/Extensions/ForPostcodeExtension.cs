namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Extensions
{
    using FluentAssertions;
    using NUnit.Framework;
    using Sfa.Das.Sas.Web.Extensions;

    [TestFixture]
    public class ForPostcodeExtension
    {
        [Test]
        public void WhenPostCodeIsNull()
        {
            PostcodeExtension.FormatPostcode(null).Should().BeEquivalentTo(string.Empty);
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("n", "N")]
        [TestCase("n1", "N1")]
        [TestCase("N17", "N17")]
        [TestCase("n170", "N170")]
        [TestCase("n170A", "N1 70A")]
        [TestCase("n170Ap", "N17 0AP")]
        [TestCase("  n170  Ap  ", "N17 0AP", Description = "Should clear all space.")]
        [TestCase("SW1W0NY", "SW1W 0NY")]
        [TestCase("n170ApEE", "N170APEE", Description = "A postcode can have a length of maximum 8 charactes including space.")]
        public void WhenPostCodeIsFormated(string input, string expected)
        {
            input.FormatPostcode().Should().BeEquivalentTo(expected);
        }
    }
}
