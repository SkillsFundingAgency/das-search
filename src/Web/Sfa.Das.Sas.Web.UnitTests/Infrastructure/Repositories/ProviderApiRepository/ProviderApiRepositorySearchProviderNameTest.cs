using System.Collections.Generic;
using FluentAssertions;
using Sfa.Das.FatApi.Client.Model;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Repositories
{
    using System.Threading.Tasks;
    using Moq;
    using NUnit.Framework;
    using Sas.Infrastructure.Repositories;
    using SFA.DAS.Apprenticeships.Api.Types.Providers;

    [TestFixture]
    public class ProviderApiRepositorySearchProviderNameTests : ProviderApiRepositoryBase
    {

        [SetUp]
        public void Setup()
        {
            _mockSearchVApi.Setup(x => x.SearchProviderNameAsync("keyword", It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new SFADASApprenticeshipsApiTypesV3ProviderSearchResults(){
            PageNumber = 1,
            PageSize = 20,
            Results = new List<SFADASApprenticeshipsApiTypesV3ProviderNameSearchResultItem>()
            });

            _mockProviderNameMapping.Setup(x => x.Map(It.IsAny<SFADASApprenticeshipsApiTypesV3ProviderSearchResults>(), It.IsAny<string>())).Returns(new ProviderNameSearchResultsAndPagination()
            {
                ResponseCode = ProviderNameSearchResponseCodes.Success,
                ActualPage = 1,
                LastPage = 10,
                SearchTerm = "keyword"
            });
       
            _sut = new ProviderApiRepository(_mockProviderApiClient.Object, _mockProviderV3ApiClient.Object, _mockSearchResultsMapping.Object,_mockLogger.Object,_mockSearchVApi.Object, _mockSearchV4Api.Object,_mockProviderNameMapping.Object);
        }
        [Test]
        public async Task ShouldBeOfTypeProviderNameSearchResultsAndPagination()
        {
            var result = await _sut.SearchProviderNameAndAliases("keyword", 1,20);
            result.Should().BeOfType<ProviderNameSearchResultsAndPagination>();
        }

        [TestCase("")]
        [TestCase("k")]
        [TestCase("ke")]
        public async Task WhenKeywordLessThanThreeCharactersThenShouldReturnSearchTermTooShortResponse(string searchTerm)
        {
            var result = await _sut.SearchProviderNameAndAliases(searchTerm, 1, 20);
            result.Should().BeOfType<ProviderNameSearchResultsAndPagination>();

            result.ResponseCode.Should().Be(ProviderNameSearchResponseCodes.SearchTermTooShort);
        }

        [TestCase("key")]
        [TestCase("keyw")]
        public async Task WhenKeywordLessThanThreeCharactersThenShouldReturnSuccess(string searchTerm)
        {
            var result = await _sut.SearchProviderNameAndAliases(searchTerm, 1, 20);
            result.Should().BeOfType<ProviderNameSearchResultsAndPagination>();
        }

    }
}
