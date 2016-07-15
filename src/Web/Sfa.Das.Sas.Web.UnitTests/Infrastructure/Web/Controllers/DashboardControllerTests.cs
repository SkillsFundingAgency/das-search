using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Factories.Interfaces;
using Sfa.Das.Sas.Web.ViewModels;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    [TestFixture]
    public sealed class DashboardControllerTests
    {
        private Mock<IGetStandards> _mockGetStandards;
        private Mock<IGetFrameworks> _mockGetFrameworks;
        private Mock<IShortlistCollection<int>> _mockListCollection;
        private Mock<IDashboardViewModelFactory> _mockDashboardViewModelFactory;
        private Mock<IShortlistViewModelFactory> _mockShortlistViewModelFactory;
        private Mock<IShortlistApprenticeshipViewModel> _mockApprenticeshipViewModel;
        private Mock<IApprenticeshipProviderRepository> _mockApprenticeshipProviderRepository;
        private Mock<DashboardViewModel> _mockDashboardViewModel;
        private DashboardController _sut;

        [SetUp]
        public void Init()
        {
            _mockGetStandards = new Mock<IGetStandards>();
            _mockGetFrameworks = new Mock<IGetFrameworks>();
            _mockListCollection = new Mock<IShortlistCollection<int>>();
            _mockDashboardViewModelFactory = new Mock<IDashboardViewModelFactory>();
            _mockShortlistViewModelFactory = new Mock<IShortlistViewModelFactory>();
            _mockApprenticeshipViewModel = new Mock<IShortlistApprenticeshipViewModel>();
            _mockApprenticeshipProviderRepository = new Mock<IApprenticeshipProviderRepository>();
            _mockDashboardViewModel = new Mock<DashboardViewModel>();

            _sut = new DashboardController(
                _mockGetStandards.Object,
                _mockGetFrameworks.Object,
                _mockListCollection.Object,
                _mockDashboardViewModelFactory.Object,
                _mockShortlistViewModelFactory.Object,
                _mockApprenticeshipProviderRepository.Object);

            _mockShortlistViewModelFactory.Setup(
                x => x.GetShortlistViewModel(It.IsAny<Standard>()))
                .Returns(_mockApprenticeshipViewModel.Object);

            _mockShortlistViewModelFactory.Setup(
               x => x.GetShortlistViewModel(It.IsAny<Framework>()))
               .Returns(_mockApprenticeshipViewModel.Object);

            _mockApprenticeshipViewModel.Setup(x => x.Providers).Returns(new List<ShortlistProviderViewModel>());

            _mockDashboardViewModelFactory.Setup(
                x => x.GetDashboardViewModel(It.IsAny<IEnumerable<IShortlistApprenticeshipViewModel>>()))
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
                                    new ShortlistedApprenticeship { ApprenticeshipId = 45, ProvidersUkrpnAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = standardId, ProvidersUkrpnAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = 83, ProvidersUkrpnAndLocation = new List<ShortlistedProvider>() }
                                });
            _mockGetStandards.Setup(x => x.GetStandardById(It.IsAny<int>()))
                             .Returns(new Standard { StandardId = standardId });

            // Act
            var result = _sut.Overview() as ViewResult;

            // Assert
            Assert.AreEqual(_mockDashboardViewModel.Object, result?.Model);
            _mockListCollection.Verify(x => x.GetAllItems(Constants.StandardsShortListCookieName));
            _mockGetStandards.Verify(x => x.GetStandardById(It.IsAny<int>()));
            _mockShortlistViewModelFactory.Verify(x => x.GetShortlistViewModel(It.IsAny<Standard>()), Times.Exactly(3));

            _mockDashboardViewModelFactory.Verify(
                x => x.GetDashboardViewModel(It.IsAny<IEnumerable<IShortlistApprenticeshipViewModel>>()), Times.Once);
        }

        [Test]
        public void ShouldNotShowStandardsThatCannotBeFound()
        {
            // Assign
            var standardId = 3;
            _mockListCollection.Setup(x => x.GetAllItems(Constants.StandardsShortListCookieName))
                                .Returns(new[]
                                {
                                    new ShortlistedApprenticeship { ApprenticeshipId = 45, ProvidersUkrpnAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = standardId, ProvidersUkrpnAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = 83, ProvidersUkrpnAndLocation = new List<ShortlistedProvider>() }
                                });

            _mockGetStandards.Setup(x => x.GetStandardById(It.IsAny<int>()))
                             .Returns(new Standard() { StandardId = standardId });

            _mockShortlistViewModelFactory.Setup(
                x => x.GetShortlistViewModel(It.Is<Standard>(s => s.StandardId.Equals(standardId))))
                .Returns(new ShortlistFrameworkViewModel());

            _mockShortlistViewModelFactory.Setup(
                x => x.GetShortlistViewModel(It.Is<Standard>(s => s.StandardId.Equals(45))))
                .Returns(new ShortlistFrameworkViewModel());

            _mockShortlistViewModelFactory.Setup(
                x => x.GetShortlistViewModel(It.Is<Standard>(s => s.StandardId.Equals(83))))
                .Returns(new ShortlistFrameworkViewModel());

            _mockGetStandards.Setup(x => x.GetStandardById(45));
            _mockGetStandards.Setup(x => x.GetStandardById(83));

            // Act
            _sut.Overview();

            // Assert
            _mockShortlistViewModelFactory.Verify(
               x => x.GetShortlistViewModel(It.Is<Standard>(s => s.StandardId.Equals(standardId))), Times.Once);

            _mockShortlistViewModelFactory.Verify(
                x => x.GetShortlistViewModel(It.Is<Standard>(s => s.StandardId.Equals(45))), Times.Never());

            _mockShortlistViewModelFactory.Verify(
                x => x.GetShortlistViewModel(It.Is<Standard>(s => s.StandardId.Equals(83))), Times.Never());
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
                                    new ShortlistedApprenticeship { ApprenticeshipId = 45, ProvidersUkrpnAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = frameworkId, ProvidersUkrpnAndLocation = new List<ShortlistedProvider>() },
                                    new ShortlistedApprenticeship { ApprenticeshipId = 83, ProvidersUkrpnAndLocation = new List<ShortlistedProvider>() }
                                });
            _mockGetFrameworks.Setup(x => x.GetFrameworkById(It.IsAny<int>()))
                             .Returns(new Framework() { FrameworkId = frameworkId });

            // Act
            var result = _sut.Overview() as ViewResult;

            // Assert
            Assert.AreEqual(_mockDashboardViewModel.Object, result?.Model);
            _mockListCollection.Verify(x => x.GetAllItems(Constants.FrameworksShortListCookieName));
            _mockGetFrameworks.Verify(x => x.GetFrameworkById(It.IsAny<int>()));
            _mockShortlistViewModelFactory.Verify(x => x.GetShortlistViewModel(It.IsAny<Framework>()), Times.Exactly(3));

            _mockDashboardViewModelFactory.Verify(
                x => x.GetDashboardViewModel(It.IsAny<IEnumerable<IShortlistApprenticeshipViewModel>>()), Times.Once);
        }
    }
}
