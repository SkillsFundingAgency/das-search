using Sfa.Das.FatApi.Client.Api;
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

        internal ProviderApiRepository _sut;

        [SetUp]
        public void SetupBase()
        {
            _mockProviderApiClient = new Mock<IProviderApiClient>();
            _mockProviderV3ApiClient = new Mock<IProvidersVApi>();
            _mockSearchResultsMapping = new Mock<ISearchResultsMapping>();

            _sut = new ProviderApiRepository(_mockProviderApiClient.Object, _mockProviderV3ApiClient.Object, _mockSearchResultsMapping.Object);
        }
    }
}
