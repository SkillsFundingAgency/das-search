using FluentAssertions;
using Sfa.Das.FatApi.Client.Model;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ProviderApiRepositorySearchProvidersByLocationTests : ProviderApiRepositoryBase
    {
        [SetUp]
        public void Setup()
        {
            _mockProviderV3ApiClient.Setup(s =>
                    s.GetByApprenticeshipIdAndLocationAsync(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(),
                        It.IsAny<string>()))
                .ReturnsAsync(new SFADASApprenticeshipsApiTypesV3ProviderApprenticeshipLocationSearchResult() { });
            _mockSearchResultsMapping.Setup(s => s.Map(It.IsAny<SFADASApprenticeshipsApiTypesV3ProviderApprenticeshipLocationSearchResult>())).Returns(new SearchResult<ProviderSearchResultItem>());
        }

        [Test]
        public async Task ShouldBeOfTypeSearchResult()
        {
            var result = await _sut.SearchProvidersByLocation("123",new Coordinate(), 1,20,new ProviderSearchFilter());
            result.Should().BeOfType<SearchResult<ProviderSearchResultItem>>();
        }

        [Test]
        public async Task ShouldCallProvidersApi()
        {
            var result = await _sut.SearchProvidersByLocation("apiId", new Coordinate(), 1, 20, new ProviderSearchFilter());

            _mockProviderV3ApiClient.Verify(v => v.GetByApprenticeshipIdAndLocationAsync("apiId",It.IsAny<double>(),It.IsAny<double>(),It.IsAny<int>(),It.IsAny<int>(),It.IsAny<bool>(),It.IsAny<bool>(),It.IsAny<string>()),Times.Once);
        }

        [Test]
        public async Task ShouldCallMapper()
        {
            var result = await _sut.SearchProvidersByLocation("apiId", new Coordinate(), 1, 20, new ProviderSearchFilter());

            _mockSearchResultsMapping.Verify(v => v.Map(It.IsAny<SFADASApprenticeshipsApiTypesV3ProviderApprenticeshipLocationSearchResult>()), Times.Once);
        }
    }
}
