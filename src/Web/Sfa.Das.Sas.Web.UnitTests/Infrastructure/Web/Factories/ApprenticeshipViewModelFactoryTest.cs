using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Factories
{
    using System.Web.Mvc;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class ApprenticeshipViewModelFactoryTest
    {
        private const string StandardProviderResultsUrl = "/hello/standard";
        private const string FrameworkProviderResultsUrl = "/hello/framework";

        private Mock<UrlHelper> _mockUrlHelper;
        private Mock<IGetStandards> _mockGetStandards;
        private Mock<IGetFrameworks> _mockGetFrameworks;
        private Mock<IMappingService> _mockMapperService;
        private ApprenticeshipViewModelFactory _sut;

        [SetUp]
        public void SetUp()
        {
            _mockUrlHelper = new Mock<UrlHelper>();

            _mockUrlHelper.Setup(m => m.Action("StandardResults", "Provider")).Returns(StandardProviderResultsUrl);
            _mockUrlHelper.Setup(m => m.Action("Standard", "Apprenticeship", It.IsAny<object>())).Returns("/hello/StandardPrevLink/id");

            _mockUrlHelper.Setup(m => m.Action("FrameworkResults", "Provider")).Returns(FrameworkProviderResultsUrl);
            _mockUrlHelper.Setup(m => m.Action("Framework", "Apprenticeship", It.IsAny<object>())).Returns("/hello/FrameworkPrevLink/id");

            _mockGetStandards = new Mock<IGetStandards>();
            _mockGetFrameworks = new Mock<IGetFrameworks>();
            _mockMapperService = new Mock<IMappingService>();

            _sut = new ApprenticeshipViewModelFactory(
                _mockGetStandards.Object,
                _mockGetFrameworks.Object,
                _mockMapperService.Object);
        }

        [Test]
        public void ShouldPopulateStandardProviderSearchViewModel()
        {
            // Assign
            var standard = new Standard
            {
                StandardId = 10,
                NotionalEndLevel = 2,
                Title = "this is a standard"
            };
            _mockGetStandards.Setup(x => x.GetStandardById(standard.StandardId)).Returns(standard);

            // Act
            var viewModel = _sut.GetProviderSearchViewModelForStandard(standard.StandardId, _mockUrlHelper.Object);

            // Assert
            _mockGetStandards.Verify(x => x.GetStandardById(standard.StandardId), Times.Once);

            Assert.IsNotNull(viewModel);
            Assert.AreEqual(StandardProviderResultsUrl, viewModel.PostUrl);
            Assert.AreEqual(standard.StandardId, viewModel.ApprenticeshipId);
            Assert.AreEqual(standard.Title + ", level " + standard.NotionalEndLevel, viewModel.Title);
        }

        [Test]
        public void ShouldPopulateFrameworkProviderSearchViewModel()
        {
            // Assign
            var framework = new Framework
            {
                FrameworkId = 10,
                Level = 2,
                Title = "this is a standard"
            };
            _mockGetFrameworks.Setup(x => x.GetFrameworkById(framework.FrameworkId)).Returns(framework);

            // Act
            var viewModel = _sut.GetFrameworkProvidersViewModel(framework.FrameworkId, _mockUrlHelper.Object);

            // Assert
            _mockGetFrameworks.Verify(x => x.GetFrameworkById(framework.FrameworkId), Times.Once);

            Assert.IsNotNull(viewModel);
            Assert.AreEqual(FrameworkProviderResultsUrl, viewModel.PostUrl);
            Assert.AreEqual(framework.FrameworkId, viewModel.ApprenticeshipId);
            Assert.AreEqual(framework.Title + ", level " + framework.Level, viewModel.Title);
        }

        [Test]
        public void ShouldGetStandardViewModel()
        {
            // Assign
            var standard = new Standard
            {
                StandardId = 10,
                NotionalEndLevel = 2,
                Title = "this is a standard"
            };
            _mockGetStandards.Setup(x => x.GetStandardById(standard.StandardId)).Returns(standard);
            _mockMapperService.Setup(x => x.Map<Standard, StandardViewModel>(standard))
                         .Returns(new StandardViewModel());

            // Act
            _sut.GetStandardViewModel(standard.StandardId);

            // Assert
            _mockGetStandards.Verify(x => x.GetStandardById(standard.StandardId), Times.Once);
            _mockMapperService.Verify(x => x.Map<Standard, StandardViewModel>(standard), Times.Once);
        }

        [Test]
        public void ShouldGetFrameworkViewModel()
        {
            // Assign
            var framework = new Framework
            {
                FrameworkId = 10,
                Level = 2,
                Title = "this is a standard"
            };

            _mockGetFrameworks.Setup(x => x.GetFrameworkById(framework.FrameworkId)).Returns(framework);
            _mockMapperService.Setup(x => x.Map<Framework, FrameworkViewModel>(framework))
                              .Returns(new FrameworkViewModel());

            // Act
            _sut.GetFrameworkViewModel(framework.FrameworkId);

            // Assert
            _mockGetFrameworks.Verify(x => x.GetFrameworkById(framework.FrameworkId), Times.Once);
            _mockMapperService.Verify(x => x.Map<Framework, FrameworkViewModel>(framework), Times.Once);
        }

        [Test]
        public void ShouldGetApprenticeshipSearchResultsViewModel()
        {
            // Assign
            var searchResults = new ApprenticeshipSearchResults();
            _mockMapperService.Setup(x => x.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(searchResults));

            // Act
            _sut.GetSApprenticeshipSearchResultViewModel(searchResults);

            // Assert
            _mockMapperService.Verify(x => x.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(searchResults), Times.Once);
        }
    }
}