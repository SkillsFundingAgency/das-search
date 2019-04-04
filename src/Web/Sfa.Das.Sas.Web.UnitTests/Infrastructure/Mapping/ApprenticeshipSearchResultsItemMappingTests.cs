using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.FatApi.Client.Model;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Infrastructure.Mapping;
using SFA.DAS.Apprenticeships.Api.Types;
using ApprenticeshipSearchResultsItem = SFA.DAS.Apprenticeships.Api.Types.ApprenticeshipSearchResultsItem;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Mapping
{
    [TestFixture]
    public class ApprenticeshipSearchResultItemsMappingTests
    {

        private ApprenticeshipSearchResultsItemMapping _sut;
        private SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem _apprenticeshipSearchResultsItem;

        [SetUp]
        public void Setup()
        {
            _sut = new ApprenticeshipSearchResultsItemMapping();
            _apprenticeshipSearchResultsItem = new SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem()
            {
                Id = "123",
                ProgrammeType = SFADASApprenticeshipsApiTypesV2ApprenticeshipSearchResultsItem.ProgrammeTypeEnum.NUMBER_0,
                Duration = 12,
                EffectiveFrom = DateTime.Now.AddYears(-2),
                EffectiveTo = new DateTime(2020, 07, 31),
                FrameworkName = "Apprenticeship Name",
                PathwayName = "Pathway Name",
                JobRoles = new List<string>()
                {
                    "Role 1",
                    "Role 2",
                    "Role 3"
                },
                Keywords =  new List<string>(){
                    "Keyword 1",
                    "Keyword 2",
                    "Keyword 3"
                },
                Level = 3,
                Published = true

            };
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Return_Mapped_Object()
        {
           var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

           mappedObject.Should().BeOfType<ApplicationServices.Models.ApprenticeshipSearchResultsItem>();
           mappedObject.Should().NotBeNull();
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Id_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.Id.Should().Be(_apprenticeshipSearchResultsItem.Id);
        }


        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Duration_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.Duration.Should().Be(_apprenticeshipSearchResultsItem.Duration);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_EffectiveFrom_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.EffectiveFrom.Should().Be(_apprenticeshipSearchResultsItem.EffectiveFrom);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_EffectiveTo_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.EffectiveTo.Should().Be(_apprenticeshipSearchResultsItem.EffectiveTo);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_FrameworkName_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.FrameworkName.Should().Be(_apprenticeshipSearchResultsItem.FrameworkName);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_PathwayName_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.PathwayName.Should().Be(_apprenticeshipSearchResultsItem.PathwayName);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Level_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.Level.Should().Be(_apprenticeshipSearchResultsItem.Level);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_Then_Published_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.Published.Should().Be(_apprenticeshipSearchResultsItem.Published);
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_And_Has_JobRoles_Then_JobRoles_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.JobRoles.Should().HaveCount(3);
            mappedObject.JobRoles.FirstOrDefault().Should().Be(_apprenticeshipSearchResultsItem.JobRoles.FirstOrDefault());
            mappedObject.JobRoles.FirstOrDefault().Should().Be(_apprenticeshipSearchResultsItem.JobRoles.FirstOrDefault());
        }


        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_And_Has_Keywords_Then_Keywords_Is_Mapped()
        {
            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.Keywords.Should().HaveCount(3);
            mappedObject.Keywords.FirstOrDefault().Should().Be(_apprenticeshipSearchResultsItem.Keywords.FirstOrDefault());
            mappedObject.Keywords.FirstOrDefault().Should().Be(_apprenticeshipSearchResultsItem.Keywords.FirstOrDefault());
        }


        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_And_Has_No_JobRoles_Then_TitleKeyword_Is_Null()
        {
            var apprenticeshipResultsNoJobrole = _apprenticeshipSearchResultsItem;

            apprenticeshipResultsNoJobrole.JobRoles = null;

            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.JobRoles.Should().BeNull();
        }

        [Test]
        public void When_Mapping_From_List_Of_ApprenticeshipSearchResultsItem_And_Has_No_keywords_Then_keywords_Is_Null()
        {
            var apprenticeshipResultsNoKeywords = _apprenticeshipSearchResultsItem;

            apprenticeshipResultsNoKeywords.Keywords = null;

            var mappedObject = _sut.Map(_apprenticeshipSearchResultsItem);

            mappedObject.Keywords.Should().BeNull();
        }
    }
}
