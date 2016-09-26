using FluentAssertions;

namespace Sfa.Das.ApprenticeshipInfoService.UnitTests.Helpers
{
    using NUnit.Framework;
    using Sfa.Das.ApprenticeshipInfoService.Infrastructure.Helpers;

    public class ControllerHelperTests
    {
        private ControllerHelper _sut = new ControllerHelper();

        [Test]
        public void ShouldReturnOneIfPageIsNegative()
        {
            var page = _sut.GetActualPage(-1);

            page.Should().Be(1);
        }

        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(3, 3)]
        [TestCase(-3, 1)]
        [TestCase(null, 1)]
        public void ShouldReturnGivenPageOrOneIfLowerThanOne(int? pageGiven, int expected)
        {
            var page = _sut.GetActualPage(pageGiven);

            page.Should().Be(expected);
        }
    }
}
