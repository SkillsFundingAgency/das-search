namespace Sfa.Eds.Das.Web.UnitTests.Services
{
    using FluentAssertions;

    using NUnit.Framework;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    [TestFixture]
    public class ForMappingProviderResultItem
    {
        [Test]
        public void WhenTypicalLengthIsEmpty()
        {
            MappingService mappingService = new MappingService(null);

            var providerResult = new StandardProviderSearchResultsItem{ EmployerSatisfaction = null, LearnerSatisfaction = 83.9 };

            var viewModel = mappingService.Map<StandardProviderSearchResultsItem, ProviderResultItemViewModel>(providerResult);

            viewModel.EmployerSatisfactionMessage.Should().Be("No data available");
            viewModel.LearnerSatisfactionMessage.Should().Be("83.9%");
        }
    }
}
