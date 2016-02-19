using FluentAssertions;
using NUnit.Framework;
using Sfa.DedsService.Services;
using Sfa.Eds.Das.Indexer.Settings.Configuration;
using Sfa.Eds.Indexer.AzureWorkerRole.DependencyResolution;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.IntegrationTests.Services
{
    [TestFixture]
    public class DedsServiceTests
    {
        private IContainer _ioc;
        private IStandardIndexSettings _standardSettings;
        private IDedsService _sut;

        [SetUp]
        public void Setup()
        {
            _ioc = IoC.Initialize();
            _standardSettings = _ioc.GetInstance<IStandardIndexSettings>();

            _sut = _ioc.GetInstance<IDedsService>();
        }

        [Test]
        [Category("Integration")]
        public void ShouldReturnNotationLevelWhenStandardIdExists()
        {
            var expectedNotationLevel = 4;

            var notationLevel = _sut.GetNotationLevelFromLars(1);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }

        [Test]
        [Category("Integration")]
        public void ShouldReturnZeroWhenStandardIdDoesntExists()
        {
            var expectedNotationLevel = 0;

            var notationLevel = _sut.GetNotationLevelFromLars(int.MaxValue);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }

        [Test]
        [Category("Integration")]
        public void ShouldReturnZeroWhenStandardIdIsNegative()
        {
            var expectedNotationLevel = 0;

            var notationLevel = _sut.GetNotationLevelFromLars(int.MinValue);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }
    }
}
