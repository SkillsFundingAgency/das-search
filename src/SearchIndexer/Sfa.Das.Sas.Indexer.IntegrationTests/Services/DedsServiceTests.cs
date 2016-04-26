using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.AzureWorkerRole.DependencyResolution;
using Sfa.Das.Sas.Indexer.Core.Services;
using StructureMap;

namespace Sfa.Das.Sas.Indexer.IntegrationTests.Services
{
    [TestFixture]
    public class DedsServiceTests
    {
        private IContainer _ioc;

        private IGetStandardLevel _sut;

        [SetUp]
        public void Setup()
        {
            _ioc = IoC.Initialize();
            _ioc.GetInstance<IIndexSettings<IMaintainApprenticeshipIndex>>();

            _sut = _ioc.GetInstance<IGetStandardLevel>();
        }

        [Test]
        [Category("Integration")]
        [Ignore]
        public void ShouldReturnNotationLevelWhenStandardIdExists()
        {
            var expectedNotationLevel = 4;

            var notationLevel = _sut.GetNotationLevel(1);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }

        [Test]
        [Category("Integration")]
        [Ignore]
        public void ShouldReturnZeroWhenStandardIdDoesntExists()
        {
            var expectedNotationLevel = 0;

            var notationLevel = _sut.GetNotationLevel(int.MaxValue);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }

        [Test]
        [Category("Integration")]
        [Ignore]
        public void ShouldReturnZeroWhenStandardIdIsNegative()
        {
            var expectedNotationLevel = 0;

            var notationLevel = _sut.GetNotationLevel(int.MinValue);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }
    }
}
