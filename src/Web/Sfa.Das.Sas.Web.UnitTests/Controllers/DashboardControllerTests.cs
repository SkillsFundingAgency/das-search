using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.ViewModels;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Controllers
{
    [TestFixture]
    public sealed class DashboardControllerTests
    {
        private Mock<IGetStandards> _mockGetStandards;
        private Mock<IGetFrameworks> _mockGetFrameworks;
        private Mock<IListCollection<int>> _mockListCollection;
        private Mock<IDashboardViewModelFactory> _mockDashboardViewModelFactory;
        private Mock<IShortlistStandardViewModelFactory> _mockShortlistStandardViewModelFactory;
        private Mock<IShortlistFrameworkViewModelFactory> _mockShortlistFrameworkViewModelFactory;
        private Mock<IApprenticeshipProviderRepository> _mockApprenticeshipProviderRepository;
        private Mock<DashboardViewModel> _mockDashboardViewModel;
        private DashboardController _sut;

        [SetUp]
        public void Init()
        {
            _mockGetStandards = new Mock<IGetStandards>();
            _mockGetFrameworks = new Mock<IGetFrameworks>();
            _mockListCollection = new Mock<IListCollection<int>>();
            _mockDashboardViewModelFactory = new Mock<IDashboardViewModelFactory>();
            _mockShortlistStandardViewModelFactory = new Mock<IShortlistStandardViewModelFactory>();
            _mockShortlistFrameworkViewModelFactory = new Mock<IShortlistFrameworkViewModelFactory>();
            _mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();
            _mockDashboardViewModel = new Mock<DashboardViewModel>();

            _sut = new DashboardController(
                _mockGetStandards.Object,
                _mockGetFrameworks.Object,
                _mockListCollection.Object,
                _mockDashboardViewModelFactory.Object,
                _mockShortlistStandardViewModelFactory.Object,
                _mockShortlistFrameworkViewModelFactory.Object,
                _mockApprenticeshipProviderRepository.Object);

            _mockShortlistStandardViewModelFactory.Setup(
                x => x.GetShortlistStandardViewModel(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new Mock<ShortlistStandardViewModel>().Object);

            _mockShortlistFrameworkViewModelFactory.Setup(
              x => x.GetShortlistFrameworkViewModel(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
              .Returns(new Mock<ShortlistFrameworkViewModel>().Object);

            _mockDashboardViewModelFactory.Setup(
                x => x.GetDashboardViewModel(
                    It.IsAny<ICollection<ShortlistStandardViewModel>>(),
                    It.IsAny<ICollection<ShortlistFrameworkViewModel>>()))
                .Returns(_mockDashboardViewModel.Object);
        }

        [Test]
        public void ShouldAddStandardViewModelsToView()
        {
            // Assign
            var standardId = 3;
            _mockListCollection.Setup(x => x.GetAllItems(Constants.StandardsShortListCookieName))
                                .Returns(new[]
                                {
                                    new ShortlistedApprenticeship { ApprenticeshipId = 45, ProvidersIdAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = standardId, ProvidersIdAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = 83, ProvidersIdAndLocation = new List<ShortlistedProvider>() }
                                });
            _mockGetStandards.Setup(x => x.GetStandardById(It.IsAny<int>()))
                             .Returns(new Standard() { StandardId = standardId });

            _mockShortlistStandardViewModelFactory.Setup(x => x.GetShortlistStandardViewModel(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new ShortlistStandardViewModel());
            // Act
            var result = _sut.Overview() as ViewResult;

            // Assert
            Assert.AreEqual(_mockDashboardViewModel.Object, result?.Model);
            _mockListCollection.Verify(x => x.GetAllItems(Constants.StandardsShortListCookieName));
            _mockGetStandards.Verify(x => x.GetStandardById(It.IsAny<int>()));
            _mockShortlistStandardViewModelFactory.Verify(
                 x => x.GetShortlistStandardViewModel(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(3));

            _mockDashboardViewModelFactory.Verify(
                x => x.GetDashboardViewModel(
                    It.IsAny<ICollection<ShortlistStandardViewModel>>(),
                    It.IsAny<ICollection<ShortlistFrameworkViewModel>>()), Times.Once);
        }

        [Test]
        public void ShouldNotShowStandardsThatCannotBeFound()
        {
            // Assign
            var standardId = 3;
            _mockListCollection.Setup(x => x.GetAllItems(Constants.StandardsShortListCookieName))
                                .Returns(new[]
                                {
                                    new ShortlistedApprenticeship { ApprenticeshipId = 45, ProvidersIdAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = standardId, ProvidersIdAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = 83, ProvidersIdAndLocation = new List<ShortlistedProvider>() }
                                });

            _mockGetStandards.Setup(x => x.GetStandardById(It.IsAny<int>()))
                             .Returns(new Standard() { StandardId = standardId });

            _mockShortlistStandardViewModelFactory.Setup(
                x => x.GetShortlistStandardViewModel(standardId, It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new ShortlistStandardViewModel());

            _mockShortlistStandardViewModelFactory.Setup(
                x => x.GetShortlistStandardViewModel(45, It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new ShortlistStandardViewModel());

            _mockShortlistStandardViewModelFactory.Setup(
                x => x.GetShortlistStandardViewModel(83, It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new ShortlistStandardViewModel());

            _mockGetStandards.Setup(x => x.GetStandardById(45));
            _mockGetStandards.Setup(x => x.GetStandardById(83));

            // Act
            _sut.Overview();

            // Assert
            _mockShortlistStandardViewModelFactory.Verify(
               x => x.GetShortlistStandardViewModel(standardId, It.IsAny<string>(), It.IsAny<int>()), Times.Once);

            _mockShortlistStandardViewModelFactory.Verify(
                x => x.GetShortlistStandardViewModel(45, It.IsAny<string>(), It.IsAny<int>()), Times.Never());

            _mockShortlistStandardViewModelFactory.Verify(
                x => x.GetShortlistStandardViewModel(83, It.IsAny<string>(), It.IsAny<int>()), Times.Never());
        }

        [Test]
        public void ShouldAddFrameworkViewModelsToView()
        {
            // Assign
            var frameworkId = 3;

            _mockListCollection.Setup(x => x.GetAllItems(Constants.StandardsShortListCookieName))
                .Returns(new List<ShortlistedApprenticeship>());

            _mockListCollection.Setup(x => x.GetAllItems(Constants.FrameworksShortListCookieName))
                                .Returns(new[]
                                {
                                    new ShortlistedApprenticeship { ApprenticeshipId = 45, ProvidersIdAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = frameworkId, ProvidersIdAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = 83, ProvidersIdAndLocation = new List<ShortlistedProvider>() }
                                });
            _mockGetFrameworks.Setup(x => x.GetFrameworkById(It.IsAny<int>()))
                             .Returns(new Framework() { FrameworkId = frameworkId });
            
            // Act
            var result = _sut.Overview() as ViewResult;

            // Assert
            Assert.AreEqual(_mockDashboardViewModel.Object, result?.Model);
            _mockListCollection.Verify(x => x.GetAllItems(Constants.FrameworksShortListCookieName));
            _mockGetFrameworks.Verify(x => x.GetFrameworkById(It.IsAny<int>()));
            _mockShortlistFrameworkViewModelFactory.Verify(
                 x => x.GetShortlistFrameworkViewModel(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()), Times.Exactly(3));

            _mockDashboardViewModelFactory.Verify(
                x => x.GetDashboardViewModel(
                    It.IsAny<ICollection<ShortlistStandardViewModel>>(),
                    It.IsAny<ICollection<ShortlistFrameworkViewModel>>()), Times.Once);
        }
    }
}
