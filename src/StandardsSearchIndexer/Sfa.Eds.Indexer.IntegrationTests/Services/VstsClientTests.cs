namespace Sfa.Eds.Das.Indexer.IntegrationTests.Services
{
    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.AzureWorkerRole.DependencyResolution;
    using Sfa.Eds.Das.ProviderIndexer.Clients;

    using StructureMap;

    [TestFixture]
    public class VstsClientTests
    {
        private IContainer _ioc;
        private IActiveProviderClient _sut;

        [SetUp]
        public void Setup()
        {
            _ioc = IoC.Initialize();

            _sut = _ioc.GetInstance<IActiveProviderClient>();
        }
        [Test]
        public void should()
        {
            var contents = _sut.GetProviders();
            
        }
    }
}
