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
            _mockProviderApiClient.Setup(x => x.GetActiveApprenticeshipTrainingByProviderAsync(It.IsAny<long>(), It.IsAny<int>())).ReturnsAsync(new ApprenticeshipTrainingSummary(){Ukprn = 12345});
       
            _sut = new ProviderApiRepository(_mockProviderApiClient.Object, _mockProviderV3ApiClient.Object, _mockSearchResultsMapping.Object,_mockLogger.Object,_mockSearchVApi.Object,_mockProviderNameMapping.Object);
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
            var result = await _sut.GetApprenticeshipTrainingSummary(12345, 1);
            result.Should().BeOfType<ApprenticeshipTrainingSummary>();
        }
    }
}
