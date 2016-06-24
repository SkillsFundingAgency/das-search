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
    }
}