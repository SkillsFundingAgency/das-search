namespace Sfa.Das.Sas.Web.UnitTests.Application
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sas.ApplicationServices.Settings;
    using Sfa.Das.Sas.ApplicationServices.Handlers;
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.ApplicationServices.Queries;
    using Sfa.Das.Sas.ApplicationServices.Validators;
    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Core.Domain.Services;
    using Sfa.Das.Sas.Core.Logging;

    [TestFixture]
    public class DetailProviderHandlerTest
    {
        private Mock<IApprenticeshipProviderRepository> _mockSearchService;
        private Mock<IShortlistCollection<int>> _mockShortlistCollection;

        private Mock<IGetStandards> _mockIGetStandards;
        private Mock<IGetFrameworks> _mockIGetFrameworks;
        private Mock<ILog> _mockLogger;

        private DetailProviderHandler _handler;

        [SetUp]
        public void Setup()
        {
            _mockSearchService = new Mock<IApprenticeshipProviderRepository>();
            _mockShortlistCollection = new Mock<IShortlistCollection<int>>();
            _mockIGetStandards = new Mock<IGetStandards>();
            _mockIGetFrameworks = new Mock<IGetFrameworks>();
            _mockLogger = new Mock<ILog>();

            var providerFrameworkSearchResults = new ApprenticeshipDetails();
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(providerFrameworkSearchResults);

            _handler = new DetailProviderHandler(
                new ProviderDetailQueryValidator(new Validation()),
                _mockSearchService.Object,
                _mockShortlistCollection.Object,
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
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(stubApprenticeship);
            _mockIGetStandards.Setup(x => x.GetStandardById(1)).Returns(stubStandardProduct);
            _mockShortlistCollection.Setup(x => x.GetAllItems(It.IsAny<string>())).Returns(new List<ShortlistedApprenticeship>());

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

            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(stubApprenticeship);
            _mockIGetStandards.Setup(x => x.GetStandardById(1)).Returns(null as Standard);
            _mockShortlistCollection.Setup(x => x.GetAllItems(It.IsAny<string>())).Returns(new List<ShortlistedApprenticeship>());

            var response = _handler.Handle(message);

            _mockIGetStandards.Verify(x => x.GetStandardById(It.IsAny<int>()), Times.Once);
            response.StatusCode.Should().Be(DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound);
        }

        [Test]
        public void ShouldNotValidateIfNotPossibleToGetCourceByStandardCode()
        {
            var message = new ProviderDetailQuery { StandardCode = "1", LocationId = 5, Ukprn = 42 };

            var stubStandardProduct = new Standard { Title = "Standard1", Level = 4, };
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(null as ApprenticeshipDetails);
            _mockIGetStandards.Setup(x => x.GetStandardById(1)).Returns(stubStandardProduct);
            _mockShortlistCollection.Setup(x => x.GetAllItems(It.IsAny<string>())).Returns(new List<ShortlistedApprenticeship>());

            var response = _handler.Handle(message);

            _mockIGetStandards.Verify(x => x.GetStandardById(It.IsAny<int>()), Times.Once);
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

            _mockSearchService.Setup(x => x.GetCourseByFrameworkId(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(stubApprenticeship);
            _mockIGetFrameworks.Setup(x => x.GetFrameworkById(1)).Returns(null as Framework);
            _mockShortlistCollection.Setup(x => x.GetAllItems(It.IsAny<string>())).Returns(new List<ShortlistedApprenticeship>());

            var response = _handler.Handle(message);

            _mockIGetFrameworks.Verify(x => x.GetFrameworkById(It.IsAny<int>()), Times.Once);
            response.StatusCode.Should().Be(DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound);
        }

        [Test]
        public void ShouldNotValidateIfNotPossibleToGetCourceByFrameworkCode()
        {
            var message = new ProviderDetailQuery { FrameworkId = "1", LocationId = 5, Ukprn = 42 };

            var stubStandardProduct = new Standard { Title = "Framework1", Level = 4, };
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(null as ApprenticeshipDetails);
            _mockIGetStandards.Setup(x => x.GetStandardById(1)).Returns(stubStandardProduct);
            _mockShortlistCollection.Setup(x => x.GetAllItems(It.IsAny<string>())).Returns(new List<ShortlistedApprenticeship>());

            var response = _handler.Handle(message);

            _mockIGetFrameworks.Verify(x => x.GetFrameworkById(It.IsAny<int>()), Times.Once);
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
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(stubApprenticeship);
            _mockIGetStandards.Setup(x => x.GetStandardById(1)).Returns(stubStandardProduct);
            _mockShortlistCollection.Setup(x => x.GetAllItems(It.IsAny<string>())).Returns(new List<ShortlistedApprenticeship>());

            var response = _handler.Handle(message);

            response.ApprenticeshipDetails.Should().Be(stubApprenticeship);
            response.ApprenticeshipLevel.ShouldBeEquivalentTo("4");
            response.ApprenticeshipNameWithLevel.ShouldAllBeEquivalentTo("Standard1");
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
            _mockSearchService.Setup(x => x.GetCourseByFrameworkId(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(stubApprenticeship);
            _mockIGetFrameworks.Setup(x => x.GetFrameworkById(1)).Returns(stubStandardProduct);
            _mockShortlistCollection.Setup(x => x.GetAllItems(It.IsAny<string>())).Returns(new List<ShortlistedApprenticeship>());

            var response = _handler.Handle(message);

            response.ApprenticeshipDetails.Should().Be(stubApprenticeship);
            response.ApprenticeshipLevel.ShouldBeEquivalentTo("4");
            response.ApprenticeshipNameWithLevel.ShouldAllBeEquivalentTo("Framework1");
            response.ApprenticeshipType.ShouldBeEquivalentTo(ApprenticeshipTrainingType.Framework);
        }

        [Test]
        public void ShouldMarkAsShortlistedIfItHasBeenForAFramework()
        {
            var message = new ProviderDetailQuery { FrameworkId = "1", LocationId = 55, Ukprn = 42 };
            var stubApprenticeship = new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct { Apprenticeship = new ApprenticeshipBasic { Code = 1 } },
                Location = new Location { LocationId = 55 },
                Provider = new Provider { UkPrn = 42 }
            };

            var stubFrameworkProduct = new Framework { Title = "Framework1", Level = 4, };
            _mockSearchService.Setup(x => x.GetCourseByFrameworkId(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(stubApprenticeship);
            _mockIGetFrameworks.Setup(x => x.GetFrameworkById(1)).Returns(stubFrameworkProduct);
            _mockShortlistCollection.Setup(x => x.GetAllItems(Constants.FrameworksShortListName)).Returns(CreateTestShortlist());

            var response = _handler.Handle(message);

            response.IsShortlisted.Should().BeTrue();
        }

        [Test]
        public void ShouldMarkAsShortlistedIfItHasBeenForAStandard()
        {
            var message = new ProviderDetailQuery { StandardCode = "1", LocationId = 55, Ukprn = 42 };
            var stubApprenticeship = new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct { Apprenticeship = new ApprenticeshipBasic { Code = 1 } },
                Location = new Location { LocationId = 55 },
                Provider = new Provider { UkPrn = 42 }
            };

            var stubStandardProduct = new Standard { Title = "Standard1", Level = 4, };
            _mockSearchService.Setup(x => x.GetCourseByStandardCode(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(stubApprenticeship);
            _mockIGetStandards.Setup(x => x.GetStandardById(1)).Returns(stubStandardProduct);
            _mockShortlistCollection.Setup(x => x.GetAllItems(Constants.StandardsShortListName)).Returns(CreateTestShortlist());

            var response = _handler.Handle(message);

            response.IsShortlisted.Should().BeTrue();
        }

        private static List<ShortlistedApprenticeship> CreateTestShortlist()
        {
            return new List<ShortlistedApprenticeship>()
            {
                new ShortlistedApprenticeship
                {
                    ApprenticeshipId = 1,
                    ProvidersUkrpnAndLocation = new List<ShortlistedProvider> { new ShortlistedProvider { Ukprn = 42, LocationId = 55 } }
                }
            };
        }
    }
}
