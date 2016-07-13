using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Apprenticeship;

using System;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Apprenticeship;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views
{
    [TestFixture]
    public sealed class FrameworkDetailPageTests : ViewTestBase
    {
        [Test]
        public void ShowExpireMessage()
        {
            var detailPage = new Framework();
            var date = default(DateTime).AddYears(1881).AddMonths(8).AddDays(4);
            var viewModel = new FrameworkViewModel { Title = "title1", ExpiryDateString = date.ToString("d MMMM yyyy") };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".expire-date").Should().Contain("This apprenticeship is closed to new starters from 5 September 1882");
        }

        [Test]
        public void DoNotShowExpireMessage()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel { Title = "title1", ExpiryDateString = null };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".expire-date").Should().BeEmpty();
        }

        [Test]
        public void ShowJobRoles()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel { Title = "title1", JobRoles = new[] { "SFA master", "DAS master" } };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".job-roles ul li").Should().Be("SFA master");
            GetPartial(html, ".job-roles ul li", 2).Should().Be("DAS master");
        }

        [Test]
        public void ShouldShowEntryRequirementsWhenItsNotEmpty()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel
            {
                Title = "title1",
                EntryRequirements = "Test entry requirements"
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".entry-requirements").Should().Contain("Test entry requirements");
            GetPartial(html, ".entry-requirements").Should().Contain("Your chosen training provider can advise you about entry requirements for apprentices.");
        }

        [Test]
        public void ShouldShowStaticTextWhenEntryRequirementsAreEmpty()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel
            {
                Title = "title1"
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".entry-requirements").Should().Be("Your chosen training provider can advise you about entry requirements for apprentices.");
        }

        [Test]
        public void ShouldShowOverviewRoleWhenItsNotEmpty()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel
            {
                Title = "title1",
                FrameworkOverview = "Test framework overview"
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".overviewTitle").Should().Be("Overview of role");
            GetPartial(html, ".overviewText").Should().Be("Test framework overview");
        }

        [Test]
        public void ShouldNotShowOverviewRoleWhenItsEmpty()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel
            {
                Title = "title1"
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".overviewTitle").Should().BeEmpty();
            GetPartial(html, ".overviewText").Should().BeEmpty();
        }

        [Test]
        public void ShouldShowSuitableRolesWhenItsNotEmpty()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel
            {
                Title = "title1",
                JobRoles = new List<string> { "jobRole" }
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".job-roles").Should().Contain("jobRole");
            GetPartial(html, ".job-roles").Should().Contain("Your chosen training provider can advise you about the kinds of skills apprentices will learn.");
        }

        [Test]
        public void ShouldShowStaticTextWhenSuitableRolesAreEmpty()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel
            {
                Title = "title1"
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".job-roles").Should().Be("Your chosen training provider can advise you about the kinds of skills apprentices will learn.");
        }

        [Test]
        public void ShouldShowQualificationsWhenItsNotEmpty()
        {
            var detailPage = new Framework();
            const string competencyTitle = "Test competency qualification";
            const string knowledgeTitle = "Test knowledge qualification";
            const string combinedTitle = "Test combined qualification";

            var viewModel = new FrameworkViewModel
            {
                Title = "title1",
                CompetencyQualification = new[] { competencyTitle },
                KnowledgeQualification = new[] { knowledgeTitle },
                CombinedQualificiation = new[] { combinedTitle },
                CompletionQualifications = "Test completion qualifications"
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();

            GetPartial(html, "li").Should().Contain(competencyTitle);
            GetPartial(html, "li", 2).Should().Contain(knowledgeTitle);
            GetPartial(html, "li", 3).Should().Contain(combinedTitle);
            GetPartial(html, ".qualificationsStatic")
                .Should()
                .Be("Your chosen training provider can advise you about the qualifications open to apprentices as they can change depending on individual and other circumstances.");
        }

        [Test]
        public void ShouldShowQualificationAvoidingCompletionIfItsEmpty()
        {
            var detailPage = new Framework();
            const string competencyTitle = "Test competency qualification";
            const string knowledgeTitle = "Test knowledge qualification";
            const string combinedTitle = "Test combined qualification";

            var viewModel = new FrameworkViewModel
            {
                Title = "title1",
                CompetencyQualification = new[] { competencyTitle },
                KnowledgeQualification = new[] { knowledgeTitle },
                CombinedQualificiation = new[] { combinedTitle },
                CompletionQualifications = string.Empty
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();

            GetPartial(html, "li").Should().Contain(competencyTitle);
            GetPartial(html, "li", 2).Should().Contain(knowledgeTitle);
            GetPartial(html, "li", 3).Should().Contain(combinedTitle);
            GetPartial(html, ".completionQualifications").Should().BeNullOrEmpty();
            GetPartial(html, ".qualificationsStatic")
                .Should()
                .Be("Your chosen training provider can advise you about the qualifications open to apprentices as they can change depending on individual and other circumstances.");
        }

        [Test]
        public void ShouldShowQualificationAvoidingQualificationIfItsEmpty()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel
            {
                Title = "title1",
                CompetencyQualification = new List<string>(),
                KnowledgeQualification = new List<string>(),
                CombinedQualificiation = new List<string>(),
                CompletionQualifications = "Test completion qualifications"
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".qualifications").Should().BeEmpty();
            GetPartial(html, ".completionQualifications").Should().Be("Test completion qualifications");
            GetPartial(html, ".qualificationsStatic")
                .Should()
                .Be("Your chosen training provider can advise you about the qualifications open to apprentices as they can change depending on individual and other circumstances.");
        }

        [Test]
        public void ShouldShowStaticTextWhenQualificationsAreEmpty()
        {
            var detailPage = new Framework();
            var viewModel = new FrameworkViewModel
            {
                Title = "title1"
            };

            var html = detailPage.RenderAsHtml(viewModel).ToAngleSharp();
            GetPartial(html, ".qualifications").Should().BeEmpty();
            GetPartial(html, ".completionQualifications").Should().BeNullOrEmpty();
            GetPartial(html, ".qualificationsStatic")
                .Should()
                .Be("Your chosen training provider can advise you about the qualifications open to apprentices as they can change depending on individual and other circumstances.");
        }
    }
}