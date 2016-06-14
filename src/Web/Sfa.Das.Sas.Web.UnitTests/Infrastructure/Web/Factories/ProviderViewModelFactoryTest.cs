using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Factories
{
    [TestFixture]
    public class ProviderViewModelFactoryTest
    {
        private ProviderViewModelFactory _sut;
        private Mock<IMappingService> _mockMappingService;
        private Mock<IApprenticeshipProviderRepository> _mockProviderRepository;
        private Mock<IGetStandards> _mockGetStandards;
        private Mock<IGetFrameworks> _mockGetFrameworks;

        [SetUp]
        public void Init()
        {
            _mockMappingService = new Mock<IMappingService>();
            _mockProviderRepository = new Mock<IApprenticeshipProviderRepository>();
            _mockGetStandards = new Mock<IGetStandards>();
            _mockGetFrameworks = new Mock<IGetFrameworks>();

            _sut = new ProviderViewModelFactory(
                _mockMappingService.Object,
                _mockProviderRepository.Object,
                _mockGetStandards.Object,
                _mockGetFrameworks.Object);
        }

        [Test]
        public void ShouldGenerateStandardDetailsViewModel()
        {
            // Assert
            var criteria = new ProviderLocationSearchCriteria
            {
                StandardCode = "123",
                ProviderId = "345",
                LocationId = "12234"
            };

            var apprenticeshipDetails = new ApprenticeshipDetails
            {
                Product = new ApprenticeshipProduct
                {
                    Apprenticeship = new ApprenticeshipBasic
                    {
                        Code = int.Parse(criteria.StandardCode)
                    }
                }
            };

            var apprenticeDetailsViewModel = new ApprenticeshipDetailsViewModel()
            {
                Apprenticeship = new ApprenticeshipBasic
                {
                    Code = int.Parse(criteria.StandardCode)
                }
            };

            var standard = new Standard
            {
                StandardId = 123,
                NotionalEndLevel = 2,
                Title = "standard test title"
            };

            _mockProviderRepository.Setup(x => x.GetCourseByStandardCode(
                criteria.ProviderId,
                criteria.LocationId,
                criteria.StandardCode))
                .Returns(apprenticeshipDetails);

            _mockMappingService.Setup(x => x.Map<ApprenticeshipDetails, ApprenticeshipDetailsViewModel>(apprenticeshipDetails))
                               .Returns(apprenticeDetailsViewModel);

            _mockGetStandards.Setup(x => x.GetStandardById(apprenticeshipDetails.Product.Apprenticeship.Code))
                .Returns(standard);

            // Act
            var viewModel = _sut.GenerateDetailsViewModel(criteria);

            // Assert
            Assert.AreEqual(apprenticeshipDetails.Product.Apprenticeship.Code, viewModel.Apprenticeship.Code);
            Assert.AreEqual(standard.NotionalEndLevel.ToString(), viewModel.ApprenticeshipLevel);
            Assert.AreEqual(standard.Title, viewModel.ApprenticeshipNameWithLevel);
            Assert.AreEqual(ApprenticeshipTrainingType.Standard, viewModel.Training);
        }

        [Test]
        public void ShouldGenerateFrameworkDetailsViewModel()
        {

        }
    }
}
