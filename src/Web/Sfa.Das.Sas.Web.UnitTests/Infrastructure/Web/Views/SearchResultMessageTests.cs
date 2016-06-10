using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Apprenticeship;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views
{
    [TestFixture]
    public sealed class SearchResultMessageTests : ViewTestBase
    {
        [Test]
        public void WhenSearchResultHasErrors()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 0,
                SearchTerm = string.Empty,
                Results = new List<ApprenticeshipSearchResultItemViewModel>(),
                HasError = true
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There was a problem performing a search. Try again later.");
        }

        [Test]
        public void WhenSearchResultIsZero()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 0,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>(),
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            GetPartial(html, "p").Should().Contain("There are no apprenticeships matching your search for 'SearchTerm'");
        }

        [Test]
        public void WhenSearchResultReturnsAll()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 68,
                SearchTerm = string.Empty,
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "p").Should().Be("All apprenticeships.");
        }

        [Test]
        public void WhenSearchResultReturnsOneResult()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 1,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            var result = GetPartial(html, "p");

            result.Should().Be("There is 1 apprenticeship matching your search for 'SearchTerm'.");
        }

        [Test]
        public void WhenSearchResultReturnsMoreThanOne()
        {
            var detail = new SearchResultMessage();
            var model = new ApprenticeshipSearchResultViewModel
            {
                TotalResults = 2,
                SearchTerm = "SearchTerm",
                Results = new List<ApprenticeshipSearchResultItemViewModel>
                              {
                                  new ApprenticeshipSearchResultItemViewModel()
                              }
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();
            GetPartial(html, "p").Should().Be("There are 2 apprenticeships matching your search for 'SearchTerm'.");
        }
    }
}
