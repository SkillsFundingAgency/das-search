using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Web.UnitTests.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Dashboard;

namespace Sfa.Das.Sas.Web.UnitTests.Views.Dashboard
{
    [TestFixture]
    public sealed class OverviewTest : ViewTestBase
    {
        [Test]
        public void ShouldShowEmptyShortlistMessage()
        {
            var viewModel = new DashboardViewModel()
            {
                HasError = false,
                Standards = new List<ShortlistStandardViewModel>(),
                Title = "Test Title"
            };

            var view = new Overview();

            var html = view.RenderAsHtml(viewModel).ToAngleSharp();

            var messageElement = GetPartial(html, "#emptyShortlistMessage");
            messageElement.Should().Contain("You have no apprenticeship training shortlisted.");
        }

        [Test]
        public void ShouldNotShowAnEmptyShortlist()
        {
            var viewModel = new DashboardViewModel()
            {
                HasError = false,
                Standards = new List<ShortlistStandardViewModel>(),
                Title = "Test Title"
            };

            var view = new Overview();

            var html = view.RenderAsHtml(viewModel).ToAngleSharp();

            var messageElement = GetPartial(html, "#standardsShortlist");
            messageElement.Should().BeNull();
        }

        [Test]
        public void ShouldShowAllStandardsInShortlist()
        {
            var standardA = new ShortlistStandardViewModel
            {
                Id = 1,
                Level = 2,
                Title = "Chemical Engineering"
            };

            var standardB = new ShortlistStandardViewModel
            {
                Id = 2,
                Level = 1,
                Title = "Network Engineering"
            };

            var viewModel = new DashboardViewModel()
            {
                HasError = false,
                Standards = new List<ShortlistStandardViewModel>()
                {
                    standardA,
                    standardB
                },
                Title = "Test Title"
            };

            var view = new Overview();

            var html = view.RenderAsHtml(viewModel).ToAngleSharp();

            var messageElement = GetHtmlElement(html, "#standardsShortlist");
            messageElement.ClassList.Should().NotContain("hidden");

            var firstStandard = GetPartial(html, "#standardsShortlist tr td", 1); // element 2 is a delete link
            var secondStandard = GetPartial(html, "#standardsShortlist tr td", 3); // element 4 is a delete link

            var expectedStandardAText = string.Format("{0} (level {1})", standardA.Title, standardA.Level);
            var expectedStandardBText = string.Format("{0} (level {1})", standardB.Title, standardB.Level);

            firstStandard.Should().Contain(expectedStandardAText);
            secondStandard.Should().Contain(expectedStandardBText);
        }
    }
}
