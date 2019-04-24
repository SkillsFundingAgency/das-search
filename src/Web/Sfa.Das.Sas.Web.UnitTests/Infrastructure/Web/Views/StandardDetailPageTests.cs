using Sfa.Das.Sas.Core.Domain;

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
                Level = 6
            };
            var model2 = new StandardViewModel
            {
                Level = 4
            };
            var model3 = new StandardViewModel
            {
                Level = 8
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
                AssessmentOrganisations = new List<AssessmentOrganisation>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#no-assessment-organisations").Should().Contain("There are no end-point assessment");
        }

        [Test]
        public void ShouldShowMoreInformation()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<AssessmentOrganisation>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#more-information").Should().Contain("The Institute for Apprenticeships has more detailed information");
        }

        [Test]
        public void ShouldNotShowOrganisationDetailsWhenNoAssessmentOrganisationsPresent()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<AssessmentOrganisation>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#organisation-name").Should().BeEmpty();
        }

        [Test]
        public void ShouldNotShowOrganisationDetailsWhenAssessmentOrganisationsAreNull()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = null
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#organisation-name").Should().BeEmpty();
        }

        [Test]
        public void ShouldShowEndPointAssessmentName()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<AssessmentOrganisation> { new AssessmentOrganisation { Name = "organisation 1" } }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, ".organisation-name").Should().Be("organisation 1");
        }

        [Test]
        public void ShouldShowEndPointAssessmentPhoneNumber()
        {
            var detail = new Standard();
            var phoneNumber = "12345";
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<AssessmentOrganisation> { new AssessmentOrganisation { Phone = phoneNumber } }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, ".phone-number").Should().Be(phoneNumber);
        }

        [Test]
        public void ShouldShowEndPointAssessmentEmail()
        {
            var detail = new Standard();
            var email = "test1@test.com";
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<AssessmentOrganisation> { new AssessmentOrganisation { Email = email } }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, ".email").Should().Be(email);
        }

        [Test]
        public void ShouldNotShowNoEndPointAssessmentIOrganisationsPresent()
        {
            var detail = new Standard();
            var model = new StandardViewModel
            {
                Level = 6,
                AssessmentOrganisations = new List<AssessmentOrganisation> { new AssessmentOrganisation { Name = "organisation 1" } }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "#no-assessment-organisations").Should().BeEmpty();
        }
    }
}
