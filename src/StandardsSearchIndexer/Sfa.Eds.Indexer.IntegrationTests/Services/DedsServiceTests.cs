using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Sfa.Eds.Indexer.DedsService.Services;
using Sfa.Eds.Indexer.ProviderIndexer.Helpers;
using Sfa.Eds.Indexer.ProviderIndexer.Services;
using Sfa.Eds.Indexer.Settings.Settings;
using Sfa.Eds.Standards.Indexer.AzureWorkerRole.DependencyResolution;
using StructureMap;

namespace Sfa.Eds.Standards.Indexer.UnitTests.Services
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
