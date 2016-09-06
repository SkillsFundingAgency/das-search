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
    public sealed class StandardDetailsPageTests : ViewTestBase
    {
        [Test]
        public void ShouldShowStandardTitle()
        {
            var detail = new StandardDetails();
            var model = new StandardMetaData
            {
                Title = "Test title"
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, ".pageTitle").Should().Be(model.Title);
        }

        [Test]
        public void ShouldShowAllFields()
        {
            var detail = new StandardDetails();
            var model = new StandardMetaData
            {
                ApprenticeshipId = 3,
                Title = "Test title",
                TypicalLength = new TypicalLength
                {
                    From = 12,
                    To = 24,
                    Unit = "m"
                },
                NotionalEndLevel = 1,
                JobRoles = new List<string>
                {
                    "jobRole1",
                    "jobRole2",
                    "jobRole3"
                },
                Keywords = new List<string>
                {
                    "keyword1",
                    "keyword2",
                    "keyword3"
                },
                StandardPdfUrl = "http://www.abba.co.uk",
                AssessmentPlanPdfUrl = "http://www.aaba2.co.uk",
                EntryRequirements = "Test entry requirements",
                WhatApprenticesWillLearn = "Test what apprenticeships will learn",
                Qualifications = "Test qualifications",
                ProfessionalRegistration = "Test professional resistration",
                OverviewOfRole = "Test overview of role",
                SectorSubjectAreaTier1 = 1.0,
                SectorSubjectAreaTier2 = 2.0
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".top div:nth-child(1) div").Should().Be(model.ApprenticeshipId.ToString());
            GetPartial(html, ".top div:nth-child(2) div").Should().Be($"From {model.TypicalLength.From} to {model.TypicalLength.To} months");
            GetPartial(html, ".top div:nth-child(3) div").Should().Be(model.NotionalEndLevel.ToString());
            GetPartial(html, ".top div:nth-child(4) div").Should().Be(model.SectorSubjectAreaTier1.ToString());
            GetPartial(html, ".top div:nth-child(5) div").Should().Be(model.SectorSubjectAreaTier2.ToString());

            GetPartial(html, "#jobRoles").Should().Be("jobRole1, jobRole2, jobRole3");
            GetPartial(html, "#keywords").Should().Be("keyword1, keyword2, keyword3");
            GetPartial(html, "#standardPdfUrl").Should().Be(model.StandardPdfUrl);
            GetPartial(html, "#assessmentPlanPdfUrl").Should().Be(model.AssessmentPlanPdfUrl);
            GetPartial(html, "#entryRequirements").Should().Be(model.EntryRequirements);
            GetPartial(html, "#whatApprenticesWillLearn").Should().Be(model.WhatApprenticesWillLearn);
            GetPartial(html, "#qualifications").Should().Be(model.Qualifications);
            GetPartial(html, "#professionalRegistration").Should().Be(model.ProfessionalRegistration);
            GetPartial(html, "#overviewOfRole").Should().Be(model.OverviewOfRole);
        }

        [Test]
        public void ShouldShowMessageIfFieldIsEmpty()
        {
            var detail = new StandardDetails();
            var model = new StandardMetaData
            {
                ApprenticeshipId = 3,
                Title = string.Empty,
                TypicalLength = new TypicalLength
                {
                    From = 12,
                    To = 24,
                    Unit = "m"
                },
                NotionalEndLevel = 1,
                JobRoles = new List<string>(),
                Keywords = new List<string>(),
                StandardPdfUrl = string.Empty,
                AssessmentPlanPdfUrl = string.Empty,
                EntryRequirements = string.Empty,
                WhatApprenticesWillLearn = string.Empty,
                Qualifications = string.Empty,
                ProfessionalRegistration = string.Empty,
                OverviewOfRole = string.Empty,
                SectorSubjectAreaTier1 = 1.0,
                SectorSubjectAreaTier2 = 2.0
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, ".top div:nth-child(1) div").Should().Be(model.ApprenticeshipId.ToString());
            GetPartial(html, ".top div:nth-child(2) div").Should().Be($"From {model.TypicalLength.From} to {model.TypicalLength.To} months");
            GetPartial(html, ".top div:nth-child(3) div").Should().Be(model.NotionalEndLevel.ToString());
            GetPartial(html, ".top div:nth-child(4) div").Should().Be(model.SectorSubjectAreaTier1.ToString());
            GetPartial(html, ".top div:nth-child(5) div").Should().Be(model.SectorSubjectAreaTier2.ToString());

            GetPartial(html, "#jobRoles").Should().Be("none");
            GetPartial(html, "#keywords").Should().Be("none");
            GetPartial(html, "#standardPdfUrl").Should().Be("none");
            GetPartial(html, "#assessmentPlanPdfUrl").Should().Be("none");
            GetPartial(html, "#entryRequirements").Should().Be("none");
            GetPartial(html, "#whatApprenticesWillLearn").Should().Be("none");
            GetPartial(html, "#qualifications").Should().Be("none");
            GetPartial(html, "#professionalRegistration").Should().Be("none");
            GetPartial(html, "#overviewOfRole").Should().Be("none");
        }
    }
}