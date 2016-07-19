using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Extensions;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Controllers
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    [TestClass]
    public class ShortlistControllerTest
    {
        private const string RequestUrl = "http://www.google.co.uk";
        private ShortlistedApprenticeship _shorlistedApprenticeship;
        private ShortlistController _controller;
        private Mock<IGetStandards> _mockStandardRepository;
        private Mock<IGetFrameworks> _mockFrameworkRepository;
        private Mock<IShortlistCollection<int>> _mockCookieRepository;

        [SetUp]
        public void Init()
        {
            _mockStandardRepository = new Mock<IGetStandards>();
            _mockFrameworkRepository = new Mock<IGetFrameworks>();
            _mockCookieRepository = new Mock<IShortlistCollection<int>>();
            _shorlistedApprenticeship = new ShortlistedApprenticeship { ApprenticeshipId = 5 };

        _controller = new ShortlistController(
                new Mock<ILog>().Object,
                _mockCookieRepository.Object);

           _controller.SetRequestUrl(RequestUrl);
        }

        [Test]
        public void ShouldAddStandardToShortListIfRequested()
        {
            // Arrange
            _mockStandardRepository.Setup(x => x.GetStandardById(It.IsAny<int>())).Returns(new Standard());
            _mockCookieRepository.Setup(x => x.AddItem(Constants.StandardsShortListName, _shorlistedApprenticeship));

            // Act
            var result = _controller.AddStandard(_shorlistedApprenticeship.ApprenticeshipId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            _mockCookieRepository.Verify(x => x.AddItem(Constants.StandardsShortListName, It.IsAny<ShortlistedApprenticeship>()), Times.Once());
        }

        [Test]
        public void ShouldRemoveStandardFromShortListIfRequested()
        {
            // Arrange
            _mockStandardRepository.Setup(x => x.GetStandardById(It.IsAny<int>())).Returns(new Standard());
            _mockCookieRepository.Setup(x => x.RemoveApprenticeship(Constants.StandardsShortListName, _shorlistedApprenticeship.ApprenticeshipId));

            // Act
            var result = _controller.RemoveStandard(_shorlistedApprenticeship.ApprenticeshipId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            _mockCookieRepository.Verify(x => x.RemoveApprenticeship(Constants.StandardsShortListName, _shorlistedApprenticeship.ApprenticeshipId), Times.Once());
        }

        [Test]
        public void ShouldRemoveStandardProvider()
        {
            // Assign
            const int apprenticeshipId = 10;

            var provider = new ShortlistedProvider
            {
                Ukprn = 546,
                LocationId = 387
            };

            _mockCookieRepository.Setup(x => x.RemoveProvider(
                Constants.StandardsShortListName,
                apprenticeshipId,
                It.Is<ShortlistedProvider>(p => p.Ukprn == provider.Ukprn)));

            // Act
            _controller.RemoveStandardProvider(apprenticeshipId, provider.Ukprn, provider.LocationId);

            // Assert
            _mockCookieRepository.Verify(
                x => x.RemoveProvider(
                    Constants.StandardsShortListName,
                    apprenticeshipId,
                    It.Is<ShortlistedProvider>(p => p.Ukprn == provider.Ukprn)),
                    Times.Once);
        }

        [Test]
        public void ShouldAddFrameworkToShortListIfRequested()
        {
            // Arrange
            _mockFrameworkRepository.Setup(x => x.GetFrameworkById(It.IsAny<int>())).Returns(new Framework());
            _mockCookieRepository.Setup(x => x.AddItem(Constants.FrameworksShortListName, _shorlistedApprenticeship));

            // Act
            var result = _controller.AddFramework(_shorlistedApprenticeship.ApprenticeshipId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            _mockCookieRepository.Verify(x => x.AddItem(Constants.FrameworksShortListName, It.IsAny<ShortlistedApprenticeship>()), Times.Once());
        }

        [Test]
        public void ShouldRemoveFrameworkFromShortListIfRequested()
        {
            // Arrange
            _mockFrameworkRepository.Setup(x => x.GetFrameworkById(It.IsAny<int>())).Returns(new Framework());
            _mockCookieRepository.Setup(x => x.RemoveApprenticeship(Constants.FrameworksShortListName, _shorlistedApprenticeship.ApprenticeshipId));

            // Act
            var result = _controller.RemoveFramework(_shorlistedApprenticeship.ApprenticeshipId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            _mockCookieRepository.Verify(x => x.RemoveApprenticeship(Constants.FrameworksShortListName, _shorlistedApprenticeship.ApprenticeshipId), Times.Once());
        }

        [Test]
        public void ShouldRemoveFrameworkProvider()
        {
            // Assign
            const int apprenticeshipId = 10;

            var provider = new ShortlistedProvider
            {
                Ukprn = 546,
                LocationId = 387
            };

            _mockCookieRepository.Setup(x => x.RemoveProvider(
                Constants.FrameworksShortListName,
                apprenticeshipId,
                It.Is<ShortlistedProvider>(p => p.Ukprn == provider.Ukprn)));

            // Act
            _controller.RemoveFrameworkProvider(apprenticeshipId, provider.Ukprn, provider.LocationId);

            // Assert
            _mockCookieRepository.Verify(
                x => x.RemoveProvider(
                    Constants.FrameworksShortListName,
                    apprenticeshipId,
                    It.Is<ShortlistedProvider>(p => p.Ukprn == provider.Ukprn)),
                    Times.Once);
        }
    }
}
