using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing; 
using Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.ExtensionHelpers;
using Sfa.Das.Sas.Web.ViewModels;
using Sfa.Das.Sas.Web.Views.Provider;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Views.Provider
{
    [TestFixture]
    public class StandardResultsPage : ViewTestBase
    {
        [Test]
        public void ShouldShowRegisterLink()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
               IsLevyPayingEmployer = true,
               Hits = new List<StandardProviderResultItemViewModel>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "#register_to_manage").Should().NotBeEmpty();
        }

        [Test]
        public void ShouldNotShowRegisterLink()
        {
            var detail = new StandardResults();
            var model = new ProviderStandardSearchResultViewModel
            {
                IsLevyPayingEmployer = false,
                Hits = new List<StandardProviderResultItemViewModel>()
            };
            var html = detail.RenderAsHtml(model).ToAngleSharp();

            this.GetPartial(html, "#register_to_manage").Should().BeEmpty();
        }
    }
}
