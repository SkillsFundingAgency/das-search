namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Mapping
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Sfa.Das.FatApi.Client.Model;
    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Infrastructure.Mapping;

    [TestFixture]
    public class ApprenticeshipSearchResultsMappingTests
    {
        private Mock<IApprenticeshipSearchResultsItemMapping> _apprenticeshipSearchResultsItemMock;

        private ApprenticeshipSearchResultsMapping _sut;

        [SetUp]
        public void Setup()
        {
            _apprenticeshipSearchResultsItemMock = new Mock<IApprenticeshipSearchResultsItemMapping>(MockBehavior.Strict);
            _apprenticeshipSearchResultsItemMock.Setup(s => s.Map(It.IsAny<SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem>())).Returns(new ApplicationServices.Models.ApprenticeshipSearchResultsItem());

            _sut = new ApprenticeshipSearchResultsMapping(_apprenticeshipSearchResultsItemMock.Object);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Return_Mapped_Object()
        {
            var apprenticeshipSearchResultsItem = new List<SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem>()
            {
                new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem(),
                new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem()
            };

            var apprenticeSearchResults = new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResults()
            {
                Results = apprenticeshipSearchResultsItem,
                TotalResults = 300,
                PageNumber = 1,
                PageSize = 2,

            };

            var mappedObject = _sut.Map(apprenticeSearchResults);

            mappedObject.Should().BeOfType<ApprenticeshipSearchResults>();
            mappedObject.Should().NotBeNull();
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Total_Pages_Calculated_Correctly()
        {
            var apprenticeshipSearchResultsItem = new List<SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem>()
            {
                new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem(),
                new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem()
            };

            var apprenticeSearchResults = new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResults()
            {
                Results = apprenticeshipSearchResultsItem,
                TotalResults = 11,
                PageNumber = 1,
                PageSize = 10,

            };

            var mappedObject = _sut.Map(apprenticeSearchResults);

            mappedObject.Should().BeOfType<ApprenticeshipSearchResults>();
            mappedObject.Should().NotBeNull();
            mappedObject.LastPage.Should().Be(2);

            apprenticeSearchResults = new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResults()
            {
                Results = apprenticeshipSearchResultsItem,
                TotalResults = 10,
                PageNumber = 1,
                PageSize = 10,

            };

            mappedObject = _sut.Map(apprenticeSearchResults);

            mappedObject.Should().BeOfType<ApprenticeshipSearchResults>();
            mappedObject.Should().NotBeNull();
            mappedObject.LastPage.Should().Be(1);

            apprenticeSearchResults = new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResults()
            {
                Results = apprenticeshipSearchResultsItem,
                TotalResults = 9,
                PageNumber = 1,
                PageSize = 10,

            };

            mappedObject = _sut.Map(apprenticeSearchResults);

            mappedObject.Should().BeOfType<ApprenticeshipSearchResults>();
            mappedObject.Should().NotBeNull();
            mappedObject.LastPage.Should().Be(1);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Return_Mapped_Object_Contains_List_Of_Results()
        {
            var apprenticeshipSearchResultsItem = new List<SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem>()
            {
                new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem(),
                new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResultsItem()
            };

            var apprenticeSearchResults = new SFADASApprenticeshipsApiTypesV3ApprenticeshipSearchResults()
            {
                Results = apprenticeshipSearchResultsItem,
                TotalResults = 300,
                PageNumber = 1,
                PageSize = 2,
            };

            var mappedObject = _sut.Map(apprenticeSearchResults);

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
