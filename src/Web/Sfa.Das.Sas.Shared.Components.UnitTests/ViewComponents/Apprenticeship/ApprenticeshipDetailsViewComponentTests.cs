using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using NUnit.Framework;
using Sfa.Das.Sas.Shared.Components.ViewComponents;
using System.Threading.Tasks;
using Moq;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewComponents.ApprenticeshipDetails;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat
{
    [TestFixture]
    public class ApprenticeshipDetailsViewComponentTests : ViewComponentTestsBase
    {
        private ApprenticeshipDetailsViewComponent _sut;
        private readonly ApprenticeshipDetailQueryViewModel _apprenticeshipDetailQueryViewModel = new ApprenticeshipDetailQueryViewModel();
        private Mock<IApprenticeshipOrchestrator> _apprenticeshipOrchestratorMock;

        [SetUp]
        public void Setup()
        {
            base.Setup();

            _apprenticeshipOrchestratorMock = new Mock<IApprenticeshipOrchestrator>(MockBehavior.Strict);

            _apprenticeshipOrchestratorMock.Setup(s => s.GetFramework(It.Is<string>(g => g == "420-2-1"))).ReturnsAsync(new FrameworkDetailsViewModel(){Id = "420-2-1"});

            _apprenticeshipOrchestratorMock.Setup(s => s.GetApprenticeshipType(It.Is<string>(g => g == "420-2-1"))).Returns(ApprenticeshipType.Framework);


            _sut = new ApprenticeshipDetailsViewComponent(_apprenticeshipOrchestratorMock.Object);
            _sut.ViewComponentContext = _viewComponentContext;
        }

        [Test]
        public async Task When_Framework_Id_Is_Provided_Then_Return_Framework_View()
        {
            _apprenticeshipDetailQueryViewModel.Id = "420-2-1";

            var result = await _sut.InvokeAsync(_apprenticeshipDetailQueryViewModel) as ViewViewComponentResult;

            result.Should().BeOfType<ViewViewComponentResult>();

            result.ViewName.Should().Be("Framework");
        }

        [Test]
        public async Task When_Framework_Id_Is_Provided_Then_Return_FrameworkDetailsViewModel()
        {
            _apprenticeshipDetailQueryViewModel.Id = "420-2-1";

            var result = await _sut.InvokeAsync(_apprenticeshipDetailQueryViewModel) as ViewViewComponentResult;

            result.ViewData.Model.Should().BeOfType<FrameworkDetailsViewModel>();

        }

        [Test]
        public async Task When_Framework_Id_Is_Provided_Then_Model_Is_Populated()
        {
            _apprenticeshipDetailQueryViewModel.Id = "420-2-1";

            var result = await _sut.InvokeAsync(_apprenticeshipDetailQueryViewModel) as ViewViewComponentResult;

            var model = result.ViewData.Model as FrameworkDetailsViewModel;

            model.Id.Should().Be("420-2-1");
        }
    }
}
