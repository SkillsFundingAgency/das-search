namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services.Mapping
{
    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class ForOverallCohortResolver
    {
        [Test]
        public void OverallCohortResolverTest()
        {
            var sut = new OverallCohortResolverTestClass();
            sut.TestResolveCore(null).Should().Be("*");
            sut.TestResolveCore("-").Should().Be(null);
            sut.TestResolveCore("2").Should().Be("2");
            sut.TestResolveCore("30").Should().Be("30");
        }
    }
}
