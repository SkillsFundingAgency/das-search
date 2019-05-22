using FluentAssertions;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Moq;
    using NUnit.Framework;
    using Sas.Infrastructure.Repositories;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;

    [TestFixture]
    public class ProviderApiRepositoryGetProviderDetailTests : ProviderApiRepositoryBase
    {

        [SetUp]
        public void Setup()
        {
            _mockProviderApiClient.Setup(x => x.GetAsync(It.IsAny<long>())).Returns(Task.FromResult(new Provider()));
            _mockProviderApiClient.Setup(x => x.GetActiveApprenticeshipTrainingByProviderAsync(It.IsAny<long>(), It.IsAny<int>())).Returns(Task.FromResult(new ApprenticeshipTrainingSummary()));

            _sut = new ProviderApiRepository(_mockProviderApiClient.Object, _mockProviderV3ApiClient.Object, _mockSearchResultsMapping.Object);
        }
        [Test]
        public async Task ShouldBeOfTypeProvider()
        {
            var result = await _sut.GetProviderDetails(It.IsAny<long>());
            result.Should().BeOfType<Provider>();
        }

        [Test]
        public async Task ShouldBeOfTypeApprenticeshipTrainingSummary()
        {
            var result = await _sut.GetApprenticeshipTrainingSummary(It.IsAny<long>(), It.IsAny<int>());
            result.Should().BeOfType<ApprenticeshipTrainingSummary>();
        }
    }
}
