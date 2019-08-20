using Sfa.Das.FatApi.Client.Api;
using SFA.DAS.NLog.Logger;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Repositories
{
    using Moq;
    using NUnit.Framework;
    using Sas.Infrastructure.Repositories;
    using SFA.DAS.Providers.Api.Client;

    [TestFixture]
    public class ProviderApiRepositoryBase
    {
        internal Mock<IProviderApiClient>_mockProviderApiClient;
        internal Mock<IProvidersVApi> _mockProviderV3ApiClient;
        internal Mock<ISearchResultsMapping> _mockSearchResultsMapping;
        internal Mock<ILog> _mockLogger;
        internal Mock<ISearchVApi> _mockSearchVApi;
        internal Mock<IProviderNameSearchMapping> _mockProviderNameMapping;

        internal ProviderApiRepository _sut;

        [SetUp]
        public void SetupBase()
        {
            _mockProviderApiClient = new Mock<IProviderApiClient>();
            _mockProviderV3ApiClient = new Mock<IProvidersVApi>();
            _mockSearchResultsMapping = new Mock<ISearchResultsMapping>();
            _mockLogger = new Mock<ILog>();
            _mockSearchVApi = new Mock<ISearchVApi>();
            _mockProviderNameMapping = new Mock<IProviderNameSearchMapping>();

            _sut = new ProviderApiRepository(_mockProviderApiClient.Object, _mockProviderV3ApiClient.Object, _mockSearchResultsMapping.Object,_mockLogger.Object, _mockSearchVApi.Object, _mockProviderNameMapping.Object);
        }
    }
}
