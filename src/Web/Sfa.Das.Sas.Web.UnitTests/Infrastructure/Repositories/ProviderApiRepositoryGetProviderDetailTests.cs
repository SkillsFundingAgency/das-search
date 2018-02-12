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
    public class ProviderApiRepositoryGetProviderDetailTests
    {
        [Test]
        public async Task ShouldBeOfTypeProvider()
        {
            var mockProviderApiClient = new Mock<IProviderApiClient>();

            mockProviderApiClient.Setup(x => x.GetAsync(It.IsAny<long>())).Returns(Task.FromResult(new Provider()));
            var providerRepository = new ProviderApiRepository(mockProviderApiClient.Object);
            var result = await providerRepository.GetProviderDetails(It.IsAny<long>());
            result.Should().BeOfType<Provider>();
        }

        [Test]
        public async Task ShouldBeOfTypeApprenticeshipTrainingSummary()
        {
            var mockProviderApiClient = new Mock<IProviderApiClient>();

            mockProviderApiClient.Setup(x => x.GetActiveApprenticeshipTrainingByProviderAsync(It.IsAny<long>(), It.IsAny<int>())).Returns(Task.FromResult(new ApprenticeshipTrainingSummary()));
            var providerRepository = new ProviderApiRepository(mockProviderApiClient.Object);
            var result = await providerRepository.GetApprenticeshipTrainingSummary(It.IsAny<long>(), It.IsAny<int>());
            result.Should().BeOfType<ApprenticeshipTrainingSummary>();
        }
    }
}
