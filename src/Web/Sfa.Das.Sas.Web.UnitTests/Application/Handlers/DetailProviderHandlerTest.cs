using Sfa.Das.Sas.ApplicationServices.Responses;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Web.UnitTests.Application
{
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sfa.Das.Sas.ApplicationServices.Handlers;
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.ApplicationServices.Queries;
    using Sfa.Das.Sas.ApplicationServices.Validators;
    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Core.Domain.Services;

    [TestFixture]
    public class DetailProviderHandlerTest
    {
        private Mock<IApprenticeshipProviderRepository> _mockSearchService;

        private Mock<IGetStandards> _mockIGetStandards;
        private Mock<IGetFrameworks> _mockIGetFrameworks;
        private Mock<ILog> _mockLogger;

        private DetailProviderHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockSearchService = new Mock<IApprenticeshipProviderRepository>();
            _mockIGetStandards = new Mock<IGetStandards>();
            _mockIGetFrameworks = new Mock<IGetFrameworks>();
            _mockLogger = new Mock<ILog>();

            var providerFrameworkSearchResults = new ApprenticeshipDetails();
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(providerFrameworkSearchResults);

            _handler = new DetailProviderHandler(
                new ProviderDetailQueryValidator(new Validation()),
                _mockSearchService.Object,
                _mockIGetStandards.Object,
                _mockIGetFrameworks.Object,
                _mockLogger.Object);
        }

        [Test]
        public void ShouldNotValidateIfMissingStandardAndFrameworkCode()
        {
            var message = new ProviderDetailQuery();

            var response = _handler.Handle(message);

            response.StatusCode.Should().Be(DetailProviderResponse.ResponseCodes.InvalidInput);
        }

        [TestCase(-42, 5)]
        [TestCase(5, -42)]
        [TestCase(0, 5)]
        [TestCase(5, 0)]
        public void ShouldNotValidateIfProviderOrLocationIdIsMissing(int ukprn, int locationId)
        {
            var message = new ProviderDetailQuery { StandardCode = "1", LocationId = locationId, Ukprn = ukprn };

            var stubApprenticeship = new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct(),
                Location = new Location { LocationId = 55 },
                Provider = new Provider { UkPrn = 42 }
            };

            var stubStandardProduct = new Standard { Title = "Standard1", Level = 4, };
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(stubApprenticeship);
            _mockIGetStandards.Setup(x => x.GetStandardById("1")).Returns(stubStandardProduct);

            var response = _handler.Handle(message);

            response.StatusCode.Should().Be(DetailProviderResponse.ResponseCodes.InvalidInput);
        }

        [Test]
        public void ShouldNotValidateIfNotPossibleToGetStandard()
        {
            var message = new ProviderDetailQuery { StandardCode = "1", LocationId = 5, Ukprn = 42 };

            var stubApprenticeship = new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct(),
                Location = new Location { LocationId = 5 },
                Provider = new Provider { UkPrn = 42 }
            };

            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(stubApprenticeship);
            _mockIGetStandards.Setup(x => x.GetStandardById("1")).Returns(null as Standard);

            var response = _handler.Handle(message);

            _mockIGetStandards.Verify(x => x.GetStandardById(It.IsAny<string>()), Times.Once);
            response.StatusCode.Should().Be(DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound);
        }

        [Test]
        public void ShouldNotValidateIfNotPossibleToGetCourceByStandardCode()
        {
            var message = new ProviderDetailQuery { StandardCode = "1", LocationId = 5, Ukprn = 42 };

            var stubStandardProduct = new Standard { Title = "Standard1", Level = 4, };
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(null as ApprenticeshipDetails);
            _mockIGetStandards.Setup(x => x.GetStandardById("1")).Returns(stubStandardProduct);

            var response = _handler.Handle(message);

            _mockIGetStandards.Verify(x => x.GetStandardById(It.IsAny<string>()), Times.Once);
            response.StatusCode.Should().Be(DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound);
        }

        [Test]
        public void ShouldNotValidateIfNotPossibleToGetFramework()
        {
            var message = new ProviderDetailQuery { FrameworkId = "1", LocationId = 5, Ukprn = 42 };

            var stubApprenticeship = new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct(),
                Location = new Location { LocationId = 5 },
                Provider = new Provider { UkPrn = 42 }
            };

            _mockSearchService.Setup(x => x.GetCourseByFrameworkId(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(stubApprenticeship);
            _mockIGetFrameworks.Setup(x => x.GetFrameworkById("1")).Returns(null as Framework);

            var response = _handler.Handle(message);

            _mockIGetFrameworks.Verify(x => x.GetFrameworkById(It.IsAny<string>()), Times.Once);
            response.StatusCode.Should().Be(DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound);
        }

        [Test]
        public void ShouldNotValidateIfNotPossibleToGetCourceByFrameworkCode()
        {
            var message = new ProviderDetailQuery { FrameworkId = "1", LocationId = 5, Ukprn = 42 };

            var stubStandardProduct = new Standard { Title = "Framework1", Level = 4, };
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(null as ApprenticeshipDetails);
            _mockIGetStandards.Setup(x => x.GetStandardById("1")).Returns(stubStandardProduct);

            var response = _handler.Handle(message);

            _mockIGetFrameworks.Verify(x => x.GetFrameworkById(It.IsAny<string>()), Times.Once);
            response.StatusCode.Should().Be(DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound);
        }

        [Test]
        public void ShouldReturnAStandard()
        {
            var message = new ProviderDetailQuery { StandardCode = "1", LocationId = 55, Ukprn = 42 };

            var stubApprenticeship = new ApprenticeshipDetails
                                         {
                                             Product = new ApprenticeshipProduct(),
                                             Location = new Location { LocationId = 55 },
                                             Provider = new Provider { UkPrn = 42 }
                                         };
            var stubStandardProduct = new Standard { Title = "Standard1", Level = 4,  };
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(stubApprenticeship);
            _mockIGetStandards.Setup(x => x.GetStandardById("1")).Returns(stubStandardProduct);

            var response = _handler.Handle(message);

            response.ApprenticeshipDetails.Should().Be(stubApprenticeship);
            response.ApprenticeshipLevel.ShouldBeEquivalentTo("4");
            response.ApprenticeshipName.ShouldAllBeEquivalentTo("Standard1");
            response.ApprenticeshipType.ShouldBeEquivalentTo(ApprenticeshipTrainingType.Standard);
        }

        [Test]
        public void ShouldReturnAFramework()
        {
            var message = new ProviderDetailQuery { FrameworkId = "1", LocationId = 55, Ukprn = 42 };

            var stubApprenticeship = new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct(),
                Location = new Location { LocationId = 55 },
                Provider = new Provider { UkPrn = 42 }
            };
            var stubStandardProduct = new Framework { Title = "Framework1", Level = 4, };
            _mockSearchService.Setup(x => x.GetCourseByFrameworkId(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(stubApprenticeship);
            _mockIGetFrameworks.Setup(x => x.GetFrameworkById("1")).Returns(stubStandardProduct);

            var response = _handler.Handle(message);

            response.ApprenticeshipDetails.Should().Be(stubApprenticeship);
            response.ApprenticeshipLevel.ShouldBeEquivalentTo("4");
            response.ApprenticeshipName.ShouldAllBeEquivalentTo("Framework1");
            response.ApprenticeshipType.ShouldBeEquivalentTo(ApprenticeshipTrainingType.Framework);
        }

        [Test]
        public void ShouldReturnAStandardFromAHigherEducationInstitute()
        {
            var message = new ProviderDetailQuery { StandardCode = "1", LocationId = 55, Ukprn = 42 };

            var stubApprenticeship = new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct(),
                Location = new Location { LocationId = 55 },
                Provider = new Provider { UkPrn = 42, IsHigherEducationInstitute = true }
            };
            var stubStandardProduct = new Standard { Title = "Standard1", Level = 4, };
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(stubApprenticeship);
            _mockIGetStandards.Setup(x => x.GetStandardById("1")).Returns(stubStandardProduct);

            var response = _handler.Handle(message);

            response.ApprenticeshipDetails.Should().Be(stubApprenticeship);
            response.ApprenticeshipLevel.ShouldBeEquivalentTo("4");
            response.ApprenticeshipName.ShouldAllBeEquivalentTo("Standard1");
            response.ApprenticeshipType.ShouldBeEquivalentTo(ApprenticeshipTrainingType.Standard);
            response.ApprenticeshipDetails.Provider.IsHigherEducationInstitute.Should().BeTrue();
        }
    }
}
