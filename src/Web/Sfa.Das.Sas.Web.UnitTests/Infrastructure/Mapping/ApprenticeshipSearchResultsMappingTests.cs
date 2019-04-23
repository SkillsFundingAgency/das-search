using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Infrastructure.Mapping;
using ApprenticeshipSearchResultsItem = SFA.DAS.Apprenticeships.Api.Types.ApprenticeshipSearchResultsItem;

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
            _apprenticeshipSearchResultsItemMock.Setup(s => s.Map(It.IsAny<ApprenticeshipSearchResultsItem>())).Returns(new ApplicationServices.Models.ApprenticeshipSearchResultsItem());

            _sut = new ApprenticeshipSearchResultsMapping(_apprenticeshipSearchResultsItemMock.Object);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Return_Mapped_Object()
        {
            var ApprenticeshipSearchResultsItem = new List<ApprenticeshipSearchResultsItem>()
            {
                new ApprenticeshipSearchResultsItem(){},
                new ApprenticeshipSearchResultsItem(){}
            };

           var mappedObject = _sut.Map(ApprenticeshipSearchResultsItem);

           mappedObject.Should().BeOfType<ApprenticeshipSearchResults>();
           mappedObject.Should().NotBeNull();
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Return_Mapped_Object_Contains_List_Of_Results()
        {
            var ApprenticeshipSearchResultsItem = new List<ApprenticeshipSearchResultsItem>()
            {
                new ApprenticeshipSearchResultsItem(){},
                new ApprenticeshipSearchResultsItem(){}
            };

            var mappedObject = _sut.Map(ApprenticeshipSearchResultsItem);

            mappedObject.Results.Should().HaveCount(2);
            mappedObject.Results.FirstOrDefault().Should().BeOfType<ApplicationServices.Models.ApprenticeshipSearchResultsItem>();
        }

        [Test]
        public void When_Mapping_From_No_results_Then_Return_Mapped_Object_Contains_noResults()
        {
            var mappedObject = _sut.Map(null);

            mappedObject.Results.Should().BeNull();
        }
    }
}
