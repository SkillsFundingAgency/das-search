namespace Sfa.Eds.Das.Web.UnitTests.Services
{
    using FluentAssertions;

    using NUnit.Framework;

    using Sfa.Eds.Das.Core.Domain.Model;
    using Sfa.Eds.Das.Web.Services;
    using Sfa.Eds.Das.Web.ViewModels;

    [TestFixture]
    public class ForMappingProviderToProviderViewModel
    {
        [Test]
        public void WhenTypicalLengthIsEmpty()
        {
            MappingService mappingService = new MappingService(null);

            var providerResult = new Provider { EmployerSatisfaction = 8.3, LearnerSatisfaction = null };

            var viewModel = mappingService.Map<Provider, ProviderViewModel>(providerResult);

            viewModel.EmployerSatisfactionMessage.Should().Be("8.3%");
            viewModel.LearnerSatisfactionMessage.Should().Be("No data available");
        }
    }
}
