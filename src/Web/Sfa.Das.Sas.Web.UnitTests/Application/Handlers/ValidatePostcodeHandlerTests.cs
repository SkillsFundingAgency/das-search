using System.Threading;
using Sfa.Das.Sas.ApplicationServices.Services;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.ApplicationServices.Handlers;
    using Sas.ApplicationServices.Queries;

    [TestFixture]
    public class ValidatePostcodeHandlerTests
    {
        private Mock<IPostcodeService> _mockPostcodeIOService;
        private ValidatePostcodeHandler _handler;

        private string englandPostcode => "EE1 2EE";
        private string walesPostcode => "WW1 2WW";
        private string scotlandPostcode => "SS1 2SS";
        private string northernIrelandPostcode => "NN1 1NN";
        private string terminatedPostcode => "TT1 1TT";
        private string invalidPostcode = "NotAPostcode";

        [SetUp]
        public void Setup()
        {
           _mockPostcodeIOService = new Mock<IPostcodeService>();

           _mockPostcodeIOService.Setup(s => s.GetPostcodeStatus(englandPostcode)).ReturnsAsync("England");
           _mockPostcodeIOService.Setup(s => s.GetPostcodeStatus(walesPostcode)).ReturnsAsync("Wales");
           _mockPostcodeIOService.Setup(s => s.GetPostcodeStatus(scotlandPostcode)).ReturnsAsync("Scotland");
           _mockPostcodeIOService.Setup(s => s.GetPostcodeStatus(northernIrelandPostcode)).ReturnsAsync("Northern Ireland");
           _mockPostcodeIOService.Setup(s => s.GetPostcodeStatus(terminatedPostcode)).ReturnsAsync("Terminated");
           _mockPostcodeIOService.Setup(s => s.GetPostcodeStatus(string.Empty)).ReturnsAsync("Error");
           _mockPostcodeIOService.Setup(s => s.GetPostcodeStatus(invalidPostcode)).ReturnsAsync("Error");

            _handler = new ValidatePostcodeHandler(_mockPostcodeIOService.Object);

        }


        [Test]
        public void When_EnglandPostcode_Then_Return_True()
        {
            var response = _handler.Handle(new ValidatePostcodeQuery() {Postcode = englandPostcode}, default(CancellationToken)).Result;

            response.Should().BeTrue();
        }

        [Test]
        public void When_WalesPostcode_Then_Return_True()
        {
            var response = _handler.Handle(new ValidatePostcodeQuery() { Postcode = walesPostcode }, default(CancellationToken)).Result;

            response.Should().BeTrue();
        }

        [Test]
        public void When_ScotlandPostcode_Then_Return_True()
        {
            var response = _handler.Handle(new ValidatePostcodeQuery() { Postcode = scotlandPostcode }, default(CancellationToken)).Result;

            response.Should().BeTrue();
        }

        [Test]
        public void When_NorthernIrelandPostcode_Then_Return_True()
        {
            var response = _handler.Handle(new ValidatePostcodeQuery() { Postcode = northernIrelandPostcode }, default(CancellationToken)).Result;

            response.Should().BeTrue();
        }

        [Test]
        public void When_TerminatedPostcode_Then_Return_False()
        {
            var response = _handler.Handle(new ValidatePostcodeQuery() { Postcode = terminatedPostcode }, default(CancellationToken)).Result;

            response.Should().BeFalse();
        }

        [Test]
        public void When_InvalidPostcode_Then_Return_False()
        {
            var response = _handler.Handle(new ValidatePostcodeQuery() { Postcode = invalidPostcode }, default(CancellationToken)).Result;

            response.Should().BeFalse();
        }

        [TestCase("")]
        [TestCase( null)]
        [TestCase(" ")]
        public void When_NoValueOrNull_Then_Return_False(string postcode)
        {
            var response = _handler.Handle(new ValidatePostcodeQuery() { Postcode = postcode }, default(CancellationToken)).Result;

            response.Should().BeFalse();
        }
    }
}
