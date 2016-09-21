namespace Sfa.Das.ApprenticeshipInfoService.UnitTests.Controllers
{
    using System;
    using System.Web.Http;
    using Api.Controllers;
    using Core.Models;
    using Core.Services;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Assert = NUnit.Framework.Assert;

    [TestFixture]
    public class StandardControllerTests
    {
        private StandardController _sut;

        [SetUp]
        public void Init()
        {
            var iGetstandards = new Mock<IGetStandards>();
            iGetstandards.Setup(m => m.GetStandardById(42)).Returns(new Standard { StandardId= 42, Title = "test title" });
            _sut = new StandardController(iGetstandards.Object);
        }

        [Test]
        public void ShouldReturnStandardkNotFound()
        {
            Exception exception = null;
            try
            {
                _sut.Get(-2);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.NotNull(exception);
            Assert.IsTrue(exception.GetType() == typeof(HttpResponseException));
        }

        [Test]
        public void ShouldReturnStandard()
        {
            var standard = _sut.Get(42);

            Assert.NotNull(standard);
            standard.StandardId.Should().Be(42);
            standard.Title.Should().Be("test title");
        }
    }
}
