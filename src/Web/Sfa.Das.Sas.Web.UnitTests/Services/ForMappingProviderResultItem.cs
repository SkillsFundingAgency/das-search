using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Services
{
    [TestFixture]
    public class ForMappingProviderResultItem
    {
        [Test]
        public void WhenTypicalLengthIsEmpty()
        {
            MappingService mappingService = new MappingService(null);

            var providerResult = new StandardProviderSearchResultsItem { EmployerSatisfaction = null, LearnerSatisfaction = 83.9 };

            var viewModel = mappingService.Map<StandardProviderSearchResultsItem, ProviderResultItemViewModel>(providerResult);

            viewModel.EmployerSatisfactionMessage.Should().Be("No data available");
            viewModel.LearnerSatisfactionMessage.Should().Be("83.9%");
        }
    }
}
