using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using Sfa.Das.Sas.ApplicationServices.Models;
using ApprenticeshipSearchResultsItem = SFA.DAS.Apprenticeships.Api.Types.ApprenticeshipSearchResultsItem;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Providers.Api
{
    using Moq;
    using NUnit.Framework;
    using Sfa.Das.Sas.Infrastructure.Mapping;
    using Sfa.Das.Sas.Infrastructure.Providers;
    using SFA.DAS.Apprenticeships.Api.Client;

    public class ApiApprenticeshipSearchProviderTests
    {
        private Mock<IApprenticeshipProgrammeApiClient> _apprenticeshipProgrammeApiClientMock;
        private Mock<IApprenticeshipSearchResultsMapping> _apprenticeshipSearchResultsMappingMock;

        private ApprenticeshipsSearchApiProvider _sut;

        private ApprenticeshipSearchResults mappingReturnObject;

        [SetUp]
        public void Setup()
        {
            mappingReturnObject = new ApprenticeshipSearchResults()
            {
                Results = new List<ApplicationServices.Models.ApprenticeshipSearchResultsItem>()
            };

            _apprenticeshipProgrammeApiClientMock = new Mock<IApprenticeshipProgrammeApiClient>();
            _apprenticeshipSearchResultsMappingMock = new Mock<IApprenticeshipSearchResultsMapping>();

            _apprenticeshipProgrammeApiClientMock
                .Setup(s => s.Search(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new List<ApprenticeshipSearchResultsItem>()
                {
                    new ApprenticeshipSearchResultsItem()
                    {
                        StandardId = "123"
                    },
                    new ApprenticeshipSearchResultsItem()
                    {
                        StandardId = "234"
                    },
                    new ApprenticeshipSearchResultsItem()
                    {
                        FrameworkId = "ABC-12-34"
                    }
                });

            _apprenticeshipSearchResultsMappingMock
                .Setup(s => s.Map(It.IsAny<IEnumerable<ApprenticeshipSearchResultsItem>>()))
                .Returns(mappingReturnObject);

            _sut = new ApprenticeshipsSearchApiProvider(_apprenticeshipProgrammeApiClientMock.Object, _apprenticeshipSearchResultsMappingMock.Object);
        }

        [Test]
        public void When_Search_Is_Called_With_Keyword_Then_Keyword_Is_Passed_To_Api()
        {
            var keyword = "test keyword";

            var results = _sut.SearchByKeyword(keyword, 0, 0, 0, null);

            _apprenticeshipProgrammeApiClientMock.Verify(v => v.Search(keyword, It.IsAny<int>()));
        }

        [Test]
        public void When_Search_Is_Called_With_Page_Then_Page_Is_Passed_To_Api()
        {
            var keyword = "test keyword";
            var page = 1;

            var results = _sut.SearchByKeyword(keyword, page, 0, 0, null);

            _apprenticeshipProgrammeApiClientMock.Verify(v => v.Search(It.IsAny<string>(), page));
        }

        [Test]
        public void When_Search_Is_Called_Then_Results_Are_Mapped()
        {
            var keyword = "test keyword";
            var page = 1;

            var results = _sut.SearchByKeyword(keyword, page, 0, 0, null);

            _apprenticeshipSearchResultsMappingMock.Verify(v => v.Map(It.IsAny<IEnumerable<ApprenticeshipSearchResultsItem>>()));

            results.Should().Be(mappingReturnObject);

        }
    }
}