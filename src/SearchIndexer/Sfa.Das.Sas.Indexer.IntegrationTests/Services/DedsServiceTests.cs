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

        [TestCase(4, 1, Description = "Should return notationLevel when standard Id exists")]
        [TestCase(0, int.MaxValue, Description = "Should return zero when standard Id doesnt exists")]
        [TestCase(0, -1, Description = "Should return zero when standard Id is negative")]
        [Category("Integration")]
        [Ignore("Lars client is not in use")]
        public void WhenCallingGetNotationLevel(int expectedNotationLevel, int standardId)
        {
            var notationLevel = _sut.GetNotationLevel(standardId);

            notationLevel.Should().NotBe(null);
            notationLevel.Should().Be(expectedNotationLevel);
        }
    }
}
