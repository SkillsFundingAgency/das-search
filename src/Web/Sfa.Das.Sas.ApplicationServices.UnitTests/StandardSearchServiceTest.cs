using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Logging;

namespace Sfa.Das.Sas.ApplicationServices.UnitTests
{
    using System.Collections.Generic;

    [TestFixture]
    public sealed class StandardSearchServiceTest
    {
        private Mock<ILog> _mockLogger;
        private Mock<IApprenticeshipSearchProvider> _mockSearchProvider;
        private Mock<IPaginationSettings> _mockPaginationSettings;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILog>();
            _mockSearchProvider = new Mock<IApprenticeshipSearchProvider>();
            _mockPaginationSettings = new Mock<IPaginationSettings>();
        }

        [Test]
        public void ShouldCallWithProvidedParameters()
        {
            _mockSearchProvider.Setup(m => m.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()));
            var standardSearchProvider = new ApprenticeshipSearchService(_mockSearchProvider.Object, _mockLogger.Object, _mockPaginationSettings.Object);

            standardSearchProvider.SearchByKeyword("test", 1, 2, 0, null);

            _mockSearchProvider.Verify(x => x.SearchByKeyword("test", 1, 2, 0, null));
        }

        [Test]
        public void ShouldDefaultTakeTo10When0()
        {
            _mockSearchProvider.Setup(m => m.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<int>>()));
            _mockPaginationSettings.Setup(x => x.DefaultResultsAmount).Returns(10);
            var standardSearchProvider = new ApprenticeshipSearchService(_mockSearchProvider.Object, _mockLogger.Object, _mockPaginationSettings.Object);

            standardSearchProvider.SearchByKeyword("test", 0, 0, 0, null);

            _mockSearchProvider.Verify(x => x.SearchByKeyword("test", 0, 10, 0, null));
        }
    }
}
