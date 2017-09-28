namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views
{
    using System.Collections.Generic;
    using ExtensionHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Sas.Web.Views.Apprenticeship;
    using SFA.DAS.Apprenticeships.Api.Types.AssessmentOrgs;
    using ViewModels;

    [TestFixture]
    public sealed class StandardDetailPageTests : ViewTestBase
    {
        [Test]
        public void ShouldShowEquivalentLevel()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<Organisation>()
            };
            var model2 = new StandardViewModel
            {
                Level = 4,
                AssessmentOrganisations = new List<Organisation>()
            };
            var model3 = new StandardViewModel
            {
                Level = 8,
                AssessmentOrganisations = new List<Organisation>()
            };

            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "dd", 1).Should().Contain("bachelor's degree");

            html = detail.RenderAsHtml(model2).ToAngleSharp();
            GetPartial(html, "dd", 1).Should().Contain("certificate of higher education");

            html = detail.RenderAsHtml(model3).ToAngleSharp();
            GetPartial(html, "dd", 1).Should().Contain("doctorate");
        }

        [Test]
        public void ShouldShowNoEndPointAssessment()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<Organisation>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#no-assessment-organisations").Should().Contain("There are no end point assessment");
        }

        [Test]
        public void ShouldNotShowOrganisationDetailsWhenNoEndPointAssessment()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<Organisation>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#organisation-name").Should().BeEmpty();
        }

        [Test]
        public void ShouldShowEndPointAssessmentNameOnly()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<Organisation> { new Organisation { Name = "organisation 1" } }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#organisation-name").Should().Be("organisation 1");
        }

        [Test]
        public void ShouldNotShowNoEndPointAssessmentIOrganisationsPresent()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<Organisation> { new Organisation { Name = "organisation 1" } }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#no-assessment-organisations").Should().BeEmpty();
        }
    }
}
