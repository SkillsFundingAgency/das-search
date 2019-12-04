using System.Threading;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Handlers;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Handlers
{
    [TestFixture]
    public sealed class GetClosestLocationsHandlerTests
    {
        private Mock<IProviderSearchProvider> _mockSearchProvider;
        private Mock<ILookupLocations> _mockPostCodeLookup;
        private GetClosestLocationsHandler _handler;
        private GetClosestLocationsQuery _validQuery;
        private CoordinateResponse _postCodeResponse;

        [SetUp]
        public void Setup()
        {
            _mockSearchProvider = new Mock<IProviderSearchProvider>();
            _mockPostCodeLookup = new Mock<ILookupLocations>();

            _validQuery = new GetClosestLocationsQuery
            {
                ApprenticeshipId = "123",
                Ukprn = 12345678,
                PostCode = "AB12 3CD"
            };

            _postCodeResponse = new CoordinateResponse { Coordinate = new Coordinate { Lat = 50, Lon = 1 }, ResponseCode = LocationLookupResponse.Ok };

            _mockPostCodeLookup.Setup(x => x.GetLatLongFromPostCode(It.IsAny<string>())).ReturnsAsync(_postCodeResponse);

            _handler = new GetClosestLocationsHandler(
                new NullLogger<GetClosestLocationsHandler>(),
                new GetClosestLocationsQueryValidator(new Validation()),
                _mockSearchProvider.Object,
                _mockPostCodeLookup.Object);
        }

        [Test]
        public void ShouldThrowValidationExceptionIfApprenticeshipIdIsNull()
        {
            _validQuery.ApprenticeshipId = null;

            Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(_validQuery, default(CancellationToken)));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(9999999)]
        [TestCase(100000000)]
        public void ShouldThrowValidationExceptionIfUkprnIsInvalid(int ukprn)
        {
            _validQuery.Ukprn = ukprn;

            Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(_validQuery, default(CancellationToken)));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase("ABC1 2SD")]
        public void ShouldThrowValidationExceptionIfPostCodeIsInvalid(string postCode)
        {
            _validQuery.PostCode = postCode;

            Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(_validQuery, default(CancellationToken)));
        }


    }
}
