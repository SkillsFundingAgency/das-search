using FluentAssertions;
using NUnit.Framework;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.UnitTests.Services
{
    [TestFixture]
    public class ForMappingProviderToProviderViewModel
    {
        [Test]
        public void WhenTypicalLengthIsEmpty()
        {
            MappingService mappingService = new MappingService(null);

            var providerResult = new ProviderCourse { EmployerSatisfaction = 8.3, LearnerSatisfaction = null };

            var viewModel = mappingService.Map<ProviderCourse, ProviderCourseViewModel>(providerResult);

            viewModel.EmployerSatisfactionMessage.Should().Be("8.3%");
            viewModel.LearnerSatisfactionMessage.Should().Be("No data available");
        }
    }
}
