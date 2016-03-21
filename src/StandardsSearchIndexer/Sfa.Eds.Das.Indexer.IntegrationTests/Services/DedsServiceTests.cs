using FluentAssertions;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.IntegrationTests.Services
{
    using ApplicationServices.Settings;
    using Core.Models;

    using Sfa.Eds.Das.Indexer.Core;
    using Sfa.Eds.Das.Indexer.Core.Services;

    [TestFixture]
    public class DedsServiceTests
    {
        private IContainer _ioc;

        private IGetStandardLevel _sut;

        [SetUp]
        public void Setup()
        {
            _ioc = IoC.Initialize();
            _ioc.GetInstance<IIndexSettings<MetaDataItem>>();

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
