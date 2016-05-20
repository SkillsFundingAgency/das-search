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
using Sfa.Das.Sas.Web.ViewModels;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Controllers
{
    [TestFixture]
    public sealed class DashboardControllerTests
    {
        private Mock<IGetStandards> _mockGetStandards;
        private Mock<IListCollection<int>> _mockListCollection;
        private Mock<IDashboardViewModelFactory> _mockDashboardViewModelFactory;
        private Mock<IShortlistStandardViewModelFactory> _mockShortlistStandardViewModelFactory;
        private Mock<DashboardViewModel> _mockDashboardViewModel;
        private DashboardController _sut;

        [SetUp]
        public void Init()
        {
            _mockGetStandards = new Mock<IGetStandards>();
            _mockListCollection = new Mock<IListCollection<int>>();
            _mockDashboardViewModelFactory = new Mock<IDashboardViewModelFactory>();
            _mockShortlistStandardViewModelFactory = new Mock<IShortlistStandardViewModelFactory>();
            _mockDashboardViewModel = new Mock<DashboardViewModel>();

            _sut = new DashboardController(
                _mockGetStandards.Object,
                _mockListCollection.Object,
                _mockDashboardViewModelFactory.Object,
                _mockShortlistStandardViewModelFactory.Object);

            _mockShortlistStandardViewModelFactory.Setup(
                x => x.GetShortlistStandardViewModel(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new Mock<ShortlistStandardViewModel>().Object);

            _mockDashboardViewModelFactory.Setup(
                x => x.GetDashboardViewModel(It.IsAny<ICollection<ShortlistStandardViewModel>>()))
                .Returns(_mockDashboardViewModel.Object);
        }
        /*
        [Test]
        public void ShouldAddStandardViewModelsToView()
        {
            // Assign
            var standardId = 3;

            _mockListCollection.Setup(x => x.GetAllItems(Constants.StandardsShortListCookieName))
                                .Returns(new[] { standardId });
            _mockGetStandards.Setup(x => x.GetStandardsByIds(It.IsAny<IEnumerable<int>>()))
                             .Returns(new List<Standard>() { new Standard() { StandardId = standardId } });

            // Act
            var result = _sut.Overview() as ViewResult;

            // Assert
            Assert.AreEqual(_mockDashboardViewModel.Object, result?.Model);
            _mockListCollection.Verify(x => x.GetAllItems(Constants.StandardsShortListCookieName));
            _mockGetStandards.Verify(x => x.GetStandardsByIds(It.IsAny<IEnumerable<int>>()));
            _mockShortlistStandardViewModelFactory.Verify(
                 x => x.GetShortlistStandardViewModel(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);

            _mockDashboardViewModelFactory.Verify(
                x => x.GetDashboardViewModel(It.IsAny<ICollection<ShortlistStandardViewModel>>()), Times.Once);
        }

        [Test]
        public void ShouldNotShowStandardsThatCannotBeFound()
        {
            // Assign
            var standardId = 3;
            _mockListCollection.Setup(x => x.GetAllItems(Constants.StandardsShortListCookieName))
                                .Returns(new[] { 45, standardId, 83 });

            _mockGetStandards.Setup(x => x.GetStandardsByIds(It.IsAny<IEnumerable<int>>()))
                             .Returns(new List<Standard>() { new Standard() { StandardId = standardId } });

            _mockShortlistStandardViewModelFactory.Setup(
                x => x.GetShortlistStandardViewModel(standardId, It.IsAny<string>(), It.IsAny<int>()));

            _mockShortlistStandardViewModelFactory.Setup(
                x => x.GetShortlistStandardViewModel(45, It.IsAny<string>(), It.IsAny<int>()));

            _mockShortlistStandardViewModelFactory.Setup(
                x => x.GetShortlistStandardViewModel(83, It.IsAny<string>(), It.IsAny<int>()));

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
        }*/
    }
}
