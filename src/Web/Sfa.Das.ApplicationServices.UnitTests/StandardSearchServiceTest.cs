namespace Sfa.Das.ApplicationServices.UnitTests
{
    using Eds.Das.Core.Logging;
    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public sealed class StandardSearchServiceTest
    {
        private Mock<ILog> _mockLogger;
        private Mock<ISearchProvider> _mockSearchProvider;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILog>();
            _mockSearchProvider = new Mock<ISearchProvider>();
        }

        [Test]
        public void ShouldCallWithProvidedParameters()
        {
            _mockSearchProvider.Setup(m => m.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            var standardSearchProvider = new ApprenticeshipSearchService(_mockSearchProvider.Object, _mockLogger.Object);

            standardSearchProvider.SearchByKeyword("test", 1, 2);

            _mockSearchProvider.Verify(x => x.SearchByKeyword("test", 1, 2));
        }

        [Test]
        public void ShouldDefaultTakeTo1000When0()
        {
            _mockSearchProvider.Setup(m => m.SearchByKeyword(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
            var standardSearchProvider = new ApprenticeshipSearchService(_mockSearchProvider.Object, _mockLogger.Object);

            standardSearchProvider.SearchByKeyword("test", 0, 0);

            _mockSearchProvider.Verify(x => x.SearchByKeyword("test", 0, 1000));
        }
    }
}
