using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Controllers;
using Sfa.Das.Sas.Web.Models;
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
            _mockCookieRepository.Setup(x => x.AddItem(Constants.StandardsShortListCookieName, _shorlistedApprenticeship));

            // Act
            var result = _controller.AddStandard(_shorlistedApprenticeship.ApprenticeshipId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            _mockCookieRepository.Verify(x => x.AddItem(Constants.StandardsShortListCookieName, It.IsAny<ShortlistedApprenticeship>()), Times.Once());
        }

        [Test]
        public void ShouldRemoveStandardFromShortListIfRequested()
        {
            // Arrange
            _mockStandardRepository.Setup(x => x.GetStandardById(It.IsAny<int>())).Returns(new Standard());
            _mockCookieRepository.Setup(x => x.RemoveApprenticeship(Constants.StandardsShortListCookieName, _shorlistedApprenticeship.ApprenticeshipId));

            // Act
            var result = _controller.RemoveStandard(_shorlistedApprenticeship.ApprenticeshipId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            _mockCookieRepository.Verify(x => x.RemoveApprenticeship(Constants.StandardsShortListCookieName, _shorlistedApprenticeship.ApprenticeshipId), Times.Once());
        }

        [Test]
        public void ShouldRemoveStandardProvider()
        {
            // Assign
            const int apprenticeshipId = 10;

            var provider = new ShortlistedProvider
            {
                ProviderId = "546",
                LocationId = 387
            };

            _mockCookieRepository.Setup(x => x.RemoveProvider(
                Constants.StandardsShortListCookieName,
                apprenticeshipId,
                It.Is<ShortlistedProvider>(p => p.ProviderId.Equals(provider.ProviderId, StringComparison.Ordinal))));

            // Act
            _controller.RemoveStandardProvider(apprenticeshipId, provider.ProviderId, provider.LocationId);

            // Assert
            _mockCookieRepository.Verify(
                x => x.RemoveProvider(
                    Constants.StandardsShortListCookieName,
                    apprenticeshipId,
                    It.Is<ShortlistedProvider>(p => p.ProviderId.Equals(provider.ProviderId, StringComparison.Ordinal))),
                    Times.Once);
        }

        [Test]
        public void ShouldAddFrameworkToShortListIfRequested()
        {
            // Arrange
            _mockFrameworkRepository.Setup(x => x.GetFrameworkById(It.IsAny<int>())).Returns(new Framework());
            _mockCookieRepository.Setup(x => x.AddItem(Constants.FrameworksShortListCookieName, _shorlistedApprenticeship));

            // Act
            var result = _controller.AddFramework(_shorlistedApprenticeship.ApprenticeshipId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            _mockCookieRepository.Verify(x => x.AddItem(Constants.FrameworksShortListCookieName, It.IsAny<ShortlistedApprenticeship>()), Times.Once());
        }

        [Test]
        public void ShouldRemoveFrameworkFromShortListIfRequested()
        {
            // Arrange
            _mockFrameworkRepository.Setup(x => x.GetFrameworkById(It.IsAny<int>())).Returns(new Framework());
            _mockCookieRepository.Setup(x => x.RemoveApprenticeship(Constants.FrameworksShortListCookieName, _shorlistedApprenticeship.ApprenticeshipId));

            // Act
            var result = _controller.RemoveFramework(_shorlistedApprenticeship.ApprenticeshipId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(result);
            _mockCookieRepository.Verify(x => x.RemoveApprenticeship(Constants.FrameworksShortListCookieName, _shorlistedApprenticeship.ApprenticeshipId), Times.Once());
        }

        [Test]
        public void ShouldRemoveFrameworkProvider()
        {
            // Assign
            const int apprenticeshipId = 10;

            var provider = new ShortlistedProvider
            {
                ProviderId = "546",
                LocationId = 387
            };

            _mockCookieRepository.Setup(x => x.RemoveProvider(
                Constants.FrameworksShortListCookieName,
                apprenticeshipId,
                It.Is<ShortlistedProvider>(p => p.ProviderId.Equals(provider.ProviderId, StringComparison.Ordinal))));

            // Act
            _controller.RemoveFrameworkProvider(apprenticeshipId, provider.ProviderId, provider.LocationId);

            // Assert
            _mockCookieRepository.Verify(
                x => x.RemoveProvider(
                    Constants.FrameworksShortListCookieName,
                    apprenticeshipId,
                    It.Is<ShortlistedProvider>(p => p.ProviderId.Equals(provider.ProviderId, StringComparison.Ordinal))),
                    Times.Once);
        }
    }
}
