using FluentAssertions;
using NUnit.Framework;

namespace Sfa.Das.Sas.Resources.UnitTests
{
    [TestFixture]
    public class EquivalenveLevelTest
    {
        [TestCase("1", "GCSEs at grades D to G")]
        [TestCase("2", "GCSEs at grades A* to C")]
        [TestCase("3", "A levels at grades A to E")]
        [TestCase("4", "certificate of higher education")]
        [TestCase("5", "foundation degree")]
        [TestCase("6", "bachelor's degree")]
        [TestCase("7", "master’s degree")]
        [TestCase("8", "doctorate")]
        [TestCase("999", "", Description = "Wrong level")]
        [TestCase(null, "", Description = "Null input")]
        [TestCase("", "", Description = "Empty input")]
        public void ShouldReturnEquivalenceTextForLevel(string level, string expected)
        {
            var actual = EquivalenveLevelService.GetApprenticeshipLevel(level);

            actual.Should().Be(expected);
        }
    }
}
