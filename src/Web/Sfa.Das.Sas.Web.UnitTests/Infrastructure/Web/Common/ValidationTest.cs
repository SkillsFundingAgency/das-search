namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Common
{
    using System.Collections.Generic;
    using Core.Logging;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.Web.Services;

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
            var validation = new Validation(null);
            validation.ValidatePostcode(postCode).Should().BeTrue();
        }

        [TestCase("")]
        [TestCase("YO31 1")]
        [TestCase("gfdsdf")]
        public void ShouldNotValidatePostcodes(string postCode)
        {
            var mockLogger = new Mock<ILog>();
            var validation = new Validation(mockLogger.Object);
            validation.ValidatePostcode(postCode).Should().BeFalse();
            mockLogger.Verify(m => m.Info("Postcode not validate", It.IsAny<Dictionary<string, object>>()), Times.Once);
        }
    }
}
