using FluentAssertions;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Moq;
    using NUnit.Framework;
    using Sas.Infrastructure.Repositories;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;
    using SFA.DAS.Providers.Api.Client;

    [TestFixture]
    public class ProviderRepositoryGetProviderDetailTests
    {
        [Test]
        public async Task ShouldProvideTheMatchingNumberOfProviderSummaries()
        {
            var mockProviderApiClient = new Mock<IProviderApiClient>();

            mockProviderApiClient.Setup(x => x.GetAsync(It.IsAny<long>())).Returns(Task.FromResult(new Provider()));
            var providerRepository = new ProviderDetailRepository(mockProviderApiClient.Object);
            var result = await providerRepository.GetProviderDetails(It.IsAny<long>());
            result.Should().BeOfType<Provider>();
        }
    }
}
