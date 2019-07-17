using FluentAssertions;
using Sfa.Das.FatApi.Client.Api;
using Sfa.Das.FatApi.Client.Model;
using Sfa.Das.Sas.ApplicationServices.Models;
using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Providers.Api
{
    using Moq;
    using NUnit.Framework;
    using Sfa.Das.Sas.Infrastructure.Mapping;
    using Sfa.Das.Sas.Infrastructure.Providers;

    public class ApiApprenticeshipSearchProviderTests
    {
        private Mock<ISearchVApi> _apprenticeshipProgrammeApiClientMock;
        private Mock<IApprenticeshipSearchResultsMapping> _apprenticeshipSearchResultsMappingMock;

        private ApprenticeshipsSearchApiProvider _sut;

        private ApprenticeshipSearchResults mappingReturnObject;
        private SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResults clientResultsItems;

        [SetUp]
        public void Setup()
        {
            mappingReturnObject = new ApprenticeshipSearchResults()
            {
                Results = new List<ApplicationServices.Models.ApprenticeshipSearchResultsItem>()
            };

            clientResultsItems = new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResults()
            {
                Results = new List<SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem>() {
                new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem()
                {
                    Id = "123"
                },
                new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem()
                {
                    Id = "234"
                },
                new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem()
                {
                    Id = "ABC-12-34"
                }
                    }
            };

            _apprenticeshipProgrammeApiClientMock = new Mock<ISearchVApi>();
            _apprenticeshipSearchResultsMappingMock = new Mock<IApprenticeshipSearchResultsMapping>();

            _apprenticeshipProgrammeApiClientMock
                .Setup(s => s.SearchActiveApprenticeshipsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(clientResultsItems);

            _apprenticeshipSearchResultsMappingMock
                .Setup(s => s.Map(It.IsAny<SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResults>()))
                .Returns(mappingReturnObject);

            _sut = new ApprenticeshipsSearchApiProvider(_apprenticeshipProgrammeApiClientMock.Object, _apprenticeshipSearchResultsMappingMock.Object);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("      ")]
        public void When_Search_Is_Called_With_Blank_Keyword_Then_An_All_Search_Sent_To_Api(string blankSearchKeyword)
        {
            var results = _sut.SearchByKeyword(blankSearchKeyword, 0, 0, 0, null);

            _apprenticeshipProgrammeApiClientMock.Verify(v => v.SearchActiveApprenticeshipsAsync("*", It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));
        }

        [Test]
        public void When_Search_Is_Called_With_Keyword_Then_Keyword_Is_Passed_To_Api()
        {
            var keyword = "test keyword";

            var results = _sut.SearchByKeyword(keyword, 0, 0, 0, null);

            _apprenticeshipProgrammeApiClientMock.Verify(v => v.SearchActiveApprenticeshipsAsync(keyword, It.IsAny<int>(),It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));
        }

        [Test]
        public void When_Search_Is_Called_With_Page_Then_Page_Is_Passed_To_Api()
        {
            var keyword = "test keyword";
            var page = 1;

            var results = _sut.SearchByKeyword(keyword, page, 0, 0, null);

            _apprenticeshipProgrammeApiClientMock.Verify(v => v.SearchActiveApprenticeshipsAsync(It.IsAny<string>(), page, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));
        }

        [Test]
        public void When_Search_Is_Called_With_Page_Then_PageSize_Is_Passed_To_Api()
        {
            var keyword = "test keyword";
            var page = 1;
            var pageSize = 20;

            var results = _sut.SearchByKeyword(keyword, page, pageSize, 0, null);

            _apprenticeshipProgrammeApiClientMock.Verify(v => v.SearchActiveApprenticeshipsAsync(It.IsAny<string>(), It.IsAny<int>(), pageSize, It.IsAny<int>(), It.IsAny<string>()));
        }

        [Test]
        public void When_Search_Is_Called_With_Page_Then_Order_Is_Passed_To_Api()
        {
            var keyword = "test keyword";
            var page = 1;
            var pageSize = 20;
            var order = 2;

            var results = _sut.SearchByKeyword(keyword, page, 0, order, null);

            _apprenticeshipProgrammeApiClientMock.Verify(v => v.SearchActiveApprenticeshipsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), order, It.IsAny<string>()));
        }

        [Test]
        public void When_Search_Is_Called_With_Page_Then_SelectedLevels_Is_Passed_To_Api()
        {
            var keyword = "test keyword";
            var page = 1;
            var pageSize = 20;
            var order = 2;
            var selectedLevels = new List<int> { 1, 2 };
            var selectedLevelsString = string.Join(",", selectedLevels);
            var results = _sut.SearchByKeyword(keyword, page, 0, order, selectedLevels);

            _apprenticeshipProgrammeApiClientMock.Verify(v => v.SearchActiveApprenticeshipsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), selectedLevelsString));
        }

        [Test]
        public void When_Search_Is_Called_Then_Results_Are_Mapped()
        {
            var keyword = "test keyword";
            var page = 1;

            var results = _sut.SearchByKeyword(keyword, page, 0, 0, null);

            _apprenticeshipSearchResultsMappingMock.Verify(v => v.Map(clientResultsItems));

            results.Should().Be(mappingReturnObject);

        }
    }
}