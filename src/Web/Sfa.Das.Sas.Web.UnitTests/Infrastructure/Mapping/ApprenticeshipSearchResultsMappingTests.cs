using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.FatApi.Client.Model;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Infrastructure.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Mapping
{
    [TestFixture]
    public class ApprenticeshipSearchResultsMappingTests
    {
        private Mock<IApprenticeshipSearchResultsItemMapping> _apprenticeshipSearchResultsItemMock;

        private ApprenticeshipSearchResultsMapping _sut;

        [SetUp]
        public void Setup()
        {
            _apprenticeshipSearchResultsItemMock = new Mock<IApprenticeshipSearchResultsItemMapping>(MockBehavior.Strict);
            _apprenticeshipSearchResultsItemMock.Setup(s => s.Map(It.IsAny<SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem>())).Returns(new ApplicationServices.Models.ApprenticeshipSearchResultsItem());

            _sut = new ApprenticeshipSearchResultsMapping(_apprenticeshipSearchResultsItemMock.Object);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Return_Mapped_Object()
        {
            var ApprenticeshipSearchResultsItem = new List<SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem>()
            {
                new SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem(),
                new SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem()
            };

            var ApprenticeSearchResults = new SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResults()
            {
                Results = ApprenticeshipSearchResultsItem,
                TotalResults = 300,
                PageNumber = 1,
                PageSize = 2,

            };

            var mappedObject = _sut.Map(ApprenticeSearchResults);

            mappedObject.Should().BeOfType<ApprenticeshipSearchResults>();
            mappedObject.Should().NotBeNull();
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Return_Mapped_Object_Contains_List_Of_Results()
        {
            var ApprenticeshipSearchResultsItem = new List<SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem>()
            {
                new SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem(),
                new SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem()
            };

            var ApprenticeSearchResults = new SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResults()
            {
                Results = ApprenticeshipSearchResultsItem,
                TotalResults = 300,
                PageNumber = 1,
                PageSize = 2,
            };

            var mappedObject = _sut.Map(ApprenticeSearchResults);

            mappedObject.Results.Should().HaveCount(2);
            mappedObject.Results.FirstOrDefault().Should().BeOfType<ApplicationServices.Models.ApprenticeshipSearchResultsItem>();
        }

        [Test]
        public void When_Mapping_From_No_results_Then_Return_Mapped_Object_Contains_noResults()
        {
            var mappedObject = _sut.Map(null);

            mappedObject.Should().BeNull();
        }
    }
}
