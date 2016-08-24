using System;
using System.Collections.Generic;
using System.Globalization;
using FluentAssertions;
using RazorGenerator.Testing;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Models;
using Sfa.Das.Sas.MetadataTool.UnitTests.Extensions;
using Sfa.Das.Sas.MetadataTool.Web.Views.Apprenticeship;

namespace Sfa.Das.Sas.MetadataTool.UnitTests.Views
{
    [TestFixture]
    public sealed class FrameworkDetailsPageTests : ViewTestBase
    {
        [Test]
        public void ShouldShowStandardTitle()
        {
            var detail = new FrameworkDetails();
            var model = new FrameworkMetaData
            {
                FrameworkName = "Test framework name",
                Pathway = "Test pathway name"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, ".pageTitle").Should().Be(string.Concat(model.FrameworkName, ": ", model.Pathway));
        }

        [Test]
        public void ShouldShowAllFields()
        {
            var detail = new FrameworkDetails();
            var model = new FrameworkMetaData
            {
                ApprenticeshipId = 3,
                FrameworkName = "Test framework name",
                FrameworkCode = 1,
                FrameworkOverview = "Test framework overview",
                TypicalLength = new TypicalLength
                {
                    From = 12,
                    To = 24,
                    Unit = "m"
                },
                PathwayCode = 1,
                Pathway = "Test pathway",
                NasTitle = "Test NAS title",
                EffectiveFrom = DateTime.Today,
                EffectiveTo = DateTime.Today.AddDays(1),
                JobRoleItems = new List<JobRoleItem>
                {
                    new JobRoleItem
                    {
                        Title = "Test job role title",
                        Description = "Test job role description"
                    }
                },
                Keywords = new List<string>
                {
                    "keyword1",
                    "keyword2",
                    "keyword3"
                },
                SectorSubjectAreaTier1 = 1.0,
                SectorSubjectAreaTier2 = 2.0,
                EntryRequirements = "Test entry requirements",
                ProfessionalRegistration = "Test professional resistration",
                CompletionQualifications = "Test completion qualifications",
                CompetencyQualification = new List<string>
                {
                    "Competency qualification 1",
                    "Competency qualification 2",
                    "Competency qualification 3"
                },
                KnowledgeQualification = new List<string>
                {
                    "Knowledge qualification 1",
                    "Knowledge qualification 2",
                    "Knowledge qualification 3"
                },
                CombinedQualification = new List<string>
                {
                    "Combined qualification 1",
                    "Combined qualification 2",
                    "Combined qualification 3"
                }
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#apprenticeshipId").Should().Be(model.ApprenticeshipId.ToString());
            GetPartial(html, "#frameworkName").Should().Be(model.FrameworkName);
            GetPartial(html, "#frameworkCode").Should().Be(model.FrameworkCode.ToString());
            GetPartial(html, "#frameworkOverview").Should().Be(model.FrameworkOverview);
            GetPartial(html, "#typicalLength")
                .Should()
                .Be(string.Concat("From ", model.TypicalLength.From, " to ", model.TypicalLength.To, " ",
                    model.TypicalLength.Unit));
            GetPartial(html, "#progType").Should().Be(model.ProgType.ToString());
            GetPartial(html, "#pathwayCode").Should().Be(model.PathwayCode.ToString());
            GetPartial(html, "#pathway").Should().Be(model.Pathway);
            GetPartial(html, "#nasTitle").Should().Be(model.NasTitle);
            GetPartial(html, "#effectiveFrom").Should().Be(model.EffectiveFrom.ToString());
            GetPartial(html, "#effectiveTo").Should().Be(model.EffectiveTo.ToString());
            GetPartial(html, "#jobRoleItems").Should().Contain("Test job role title");
            GetPartial(html, "#jobRoleItems").Should().Contain("Test job role description");
            GetPartial(html, "#keywords").Should().Be("keyword1, keyword2, keyword3");
            GetPartial(html, "#sectorSubjectAreaTier1")
                .Should()
                .Be(model.SectorSubjectAreaTier1.ToString(CultureInfo.InvariantCulture));
            GetPartial(html, "#sectorSubjectAreaTier2")
                .Should()
                .Be(model.SectorSubjectAreaTier2.ToString(CultureInfo.InvariantCulture));
            GetPartial(html, "#entryRequirements").Should().Be(model.EntryRequirements);
            GetPartial(html, "#professionalRegistration").Should().Be(model.ProfessionalRegistration);
            GetPartial(html, "#completionQualifications").Should().Be(model.CompletionQualifications);
            GetPartial(html, "#competencyQualification")
                .Should()
                .Be("Competency qualification 1, Competency qualification 2, Competency qualification 3");
            GetPartial(html, "#knowledgeQualification")
                .Should()
                .Be("Knowledge qualification 1, Knowledge qualification 2, Knowledge qualification 3");
            GetPartial(html, "#combinedQualification")
                .Should()
                .Be("Combined qualification 1, Combined qualification 2, Combined qualification 3");
        }

        [Test]
        public void ShouldShowMessageIfFieldIsEmpty()
        {
            var detail = new FrameworkDetails();
            var model = new FrameworkMetaData
            {
                ApprenticeshipId = 3,
                FrameworkName = string.Empty,
                FrameworkCode = 1,
                FrameworkOverview = string.Empty,
                TypicalLength = new TypicalLength
                {
                    From = 12,
                    To = 24,
                    Unit = "m"
                },
                PathwayCode = 1,
                Pathway = string.Empty,
                NasTitle = string.Empty,
                EffectiveFrom = DateTime.Today,
                EffectiveTo = DateTime.Today.AddDays(1),
                JobRoleItems = new List<JobRoleItem>(),
                Keywords = new List<string>(),
                SectorSubjectAreaTier1 = 1.0,
                SectorSubjectAreaTier2 = 2.0,
                EntryRequirements = string.Empty,
                ProfessionalRegistration = string.Empty,
                CompletionQualifications = string.Empty,
                CompetencyQualification = new List<string>(),
                KnowledgeQualification = new List<string>(),
                CombinedQualification = new List<string>()
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "#apprenticeshipId").Should().Be(model.ApprenticeshipId.ToString());
            GetPartial(html, "#frameworkName").Should().Be("none");
            GetPartial(html, "#frameworkCode").Should().Be(model.FrameworkCode.ToString());
            GetPartial(html, "#frameworkOverview").Should().Be("none");
            GetPartial(html, "#typicalLength")
                .Should()
                .Be(string.Concat("From ", model.TypicalLength.From, " to ", model.TypicalLength.To, " ",
                    model.TypicalLength.Unit));
            GetPartial(html, "#progType").Should().Be(model.ProgType.ToString());
            GetPartial(html, "#pathwayCode").Should().Be(model.PathwayCode.ToString());
            GetPartial(html, "#pathway").Should().Be("none");
            GetPartial(html, "#nasTitle").Should().Be("none");
            GetPartial(html, "#effectiveFrom").Should().Be(model.EffectiveFrom.ToString());
            GetPartial(html, "#effectiveTo").Should().Be(model.EffectiveTo.ToString());
            GetPartial(html, "#jobRoleItems").Should().Be("none");
            GetPartial(html, "#keywords").Should().Be("none");
            GetPartial(html, "#sectorSubjectAreaTier1")
                .Should()
                .Be(model.SectorSubjectAreaTier1.ToString(CultureInfo.InvariantCulture));
            GetPartial(html, "#sectorSubjectAreaTier2")
                .Should()
                .Be(model.SectorSubjectAreaTier2.ToString(CultureInfo.InvariantCulture));
            GetPartial(html, "#entryRequirements").Should().Be("none");
            GetPartial(html, "#professionalRegistration").Should().Be("none");
            GetPartial(html, "#completionQualifications").Should().Be("none");
            GetPartial(html, "#competencyQualification").Should().Be("none");
            GetPartial(html, "#knowledgeQualification").Should().Be("none");
            GetPartial(html, "#combinedQualification").Should().Be("none");
        }
    }
}
