namespace Sfa.Das.Sas.Web.UnitTests.Common
{
    using FluentAssertions;

    using NUnit.Framework;

    using Web.Common;

    [TestFixture]
    public class ValidationTest
    {
        [TestCase("A9 9AA")]
        [TestCase("A9A 9AA")]
        [TestCase("A99 9AA")]
        [TestCase("AA9 9AA")]
        [TestCase("AA9A 9AA")]
        [TestCase("NW67XT")]
        [TestCase("W1Y 8HE")]
        [TestCase("B721HE")]
        public void ShouldValidatePostcodes(string postCode)
        {
            Validation.ValidatePostcode(postCode).Should().BeTrue();
        }

        [TestCase("")]
        [TestCase("YO31 1")]
        [TestCase("gfdsdf")]
        public void ShouldNotValidatePostcodes(string postCode)
        {
            Validation.ValidatePostcode(postCode).Should().BeFalse();
        }
    }
}
