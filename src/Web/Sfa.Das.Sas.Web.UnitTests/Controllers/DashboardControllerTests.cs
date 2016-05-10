using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Collections;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.ViewModels;
using Assert = NUnit.Framework.Assert;

namespace Sfa.Das.Sas.Web.UnitTests.Controllers
{
    [TestFixture]
    public sealed class DashboardControllerTests
    {
        private Mock<IGetStandards> _mockGetStandards;
        private Mock<IListCollection<int>> _mockListCollection;
        private DashboardController _sut;

        [SetUp]
        public void Init()
        {
            _mockGetStandards = new Mock<IGetStandards>();
            _mockListCollection = new Mock<IListCollection<int>>();

            _sut = new DashboardController(_mockGetStandards.Object, _mockListCollection.Object);
        }

        [Test]
        public void ShouldAddStandardViewModelsToView()
        {
            //Assign
            var standardId = 3;
            _mockListCollection.Setup(x => x.GetAllItems(Constants.StandardsShortListCookieName))
                                .Returns(new[] { standardId });
            _mockGetStandards.Setup(x => x.GetStandardsByIds(It.IsAny<IEnumerable<int>>()))
                             .Returns(new List<Standard>() { new Standard() { StandardId = standardId } });

            //Act
            var result = _sut.Overview() as ViewResult;
            var viewModel = result?.Model as DashboardViewModel;

            //Assert
            Assert.AreEqual(1, viewModel.Standards.Count());
            _mockListCollection.Verify(x => x.GetAllItems(Constants.StandardsShortListCookieName));
            _mockGetStandards.Verify(x => x.GetStandardsByIds(It.IsAny<IEnumerable<int>>()));
            Assert.AreEqual(standardId, viewModel.Standards.First().Id);
        }

        [Test]
        public void ShouldNotShowStandardsThatCannotBeFound()
        {
            //Assign
            var standardId = 3;
            _mockListCollection.Setup(x => x.GetAllItems(Constants.StandardsShortListCookieName))
                                .Returns(new[] { 45, standardId, 83 });

            _mockGetStandards.Setup(x => x.GetStandardsByIds(It.IsAny<IEnumerable<int>>()))
                             .Returns(new List<Standard>() { new Standard() { StandardId = standardId } });

            _mockGetStandards.Setup(x => x.GetStandardById(45));
            _mockGetStandards.Setup(x => x.GetStandardById(83));

            //Act
            var result = _sut.Overview() as ViewResult;
            var viewModel = result?.Model as DashboardViewModel;

            //Assert
            Assert.AreEqual(1, viewModel.Standards.Count());
            Assert.AreEqual(standardId, viewModel.Standards.First().Id);
        }
    }
}
