using FluentAssertions;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
using Sfa.Eds.Das.StandardIndexer.Settings;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.IntegrationTests.Services
{
    using Sfa.Eds.Das.Indexer.Core;

    [TestFixture]
    public class DedsServiceTests
    {
        private IContainer _ioc;
        private IStandardIndexSettings _standardSettings;
        private IGetStandardLevel _sut;

        [SetUp]
        public void Setup()
        {
            _ioc = IoC.Initialize();
            _standardSettings = _ioc.GetInstance<IStandardIndexSettings>();

            _sut = _ioc.GetInstance<IGetStandardLevel>();
        }

        [Test]
        [Category("Integration")]
        public void ShouldReturnNotationLevelWhenStandardIdExists()
        {
            var expectedNotationLevel = 4;

            var notationLevel = _sut.GetNotationLevel(1);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }

        [Test]
        [Category("Integration")]
        public void ShouldReturnZeroWhenStandardIdDoesntExists()
        {
            var expectedNotationLevel = 0;

            var notationLevel = _sut.GetNotationLevel(int.MaxValue);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }

        [Test]
        [Category("Integration")]
        public void ShouldReturnZeroWhenStandardIdIsNegative()
        {
            var expectedNotationLevel = 0;

            var notationLevel = _sut.GetNotationLevel(int.MinValue);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }
    }
}
