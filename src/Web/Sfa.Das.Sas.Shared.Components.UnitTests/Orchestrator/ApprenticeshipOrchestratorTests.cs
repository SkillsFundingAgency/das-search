using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using NLog.LayoutRenderers.Wrappers;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewComponents.ApprenticeshipDetails;
using SFA.DAS.NLog.Logger;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.Orchestrator
{
    [TestFixture]
    public class ApprenticeshipOrchestratorTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILog> _loggerMock;
        private Mock<IFrameworkDetailsViewModelMapper> _frameworkMapperMock;
        private ApprenticeshipOrchestrator _sut;
        private string _frameworkId = "420-2-1";

        private FrameworkDetailsViewModel _framework = new FrameworkDetailsViewModel();

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _loggerMock = new Mock<ILog>();
            _frameworkMapperMock = new Mock<IFrameworkDetailsViewModelMapper>(MockBehavior.Strict);


            _mediatorMock.Setup(s => s.Send<GetFrameworkResponse>(It.Is<GetFrameworkQuery>(request => request.Id == "420-2-1"), It.IsAny<CancellationToken>())).ReturnsAsync(new GetFrameworkResponse() { StatusCode = GetFrameworkResponse.ResponseCodes.InvalidFrameworkId });
            _mediatorMock.Setup(s => s.Send<GetFrameworkResponse>(It.Is<GetFrameworkQuery>(request => request.Id == "530-2-1"), It.IsAny<CancellationToken>())).ReturnsAsync(new GetFrameworkResponse() { StatusCode = GetFrameworkResponse.ResponseCodes.FrameworkNotFound });
            _mediatorMock.Setup(s => s.Send<GetFrameworkResponse>(It.Is<GetFrameworkQuery>(request => request.Id == "130-2-1"), It.IsAny<CancellationToken>())).ReturnsAsync(new GetFrameworkResponse() { StatusCode = GetFrameworkResponse.ResponseCodes.Gone });
            _mediatorMock.Setup(s => s.Send<GetFrameworkResponse>(It.Is<GetFrameworkQuery>(request => request.Id == "230-2-1"), It.IsAny<CancellationToken>())).ReturnsAsync(new GetFrameworkResponse() { StatusCode = GetFrameworkResponse.ResponseCodes.Success, Framework = new Framework(){ FrameworkId = "230-2-1"}});

            _frameworkMapperMock.Setup(s => s.Map(It.IsAny<Framework>())).Returns(_framework);

            _sut = new ApprenticeshipOrchestrator(_mediatorMock.Object, _loggerMock.Object, _frameworkMapperMock.Object);
        }

        [Test]
        public async Task When_Getting_Framework_And_Response_StatusCode_Is_InvalidFrameworkId_Then_Exception()
        {

            Action result = () => _sut.GetFramework(_frameworkId).Wait();

           result.Should().Throw<Exception>()
               .WithMessage("Framework id: 420-2-1 has wrong format");
        }

        [Test]
        public async Task When_Getting_Framework_And_Response_StatusCode_Is_FrameworkNotFound_Then_Exception()
        {

            Action result = () => _sut.GetFramework("530-2-1").Wait();

            result.Should().Throw<Exception>()
                .WithMessage("Cannot find framework: 530-2-1");
        }
        [Test]
        public async Task When_Getting_Framework_And_Response_StatusCode_Is_Gone_Then_Exception()
        {

            Action result = () => _sut.GetFramework("130-2-1").Wait();

            result.Should().Throw<Exception>()
                .WithMessage("Expired framework request: 130-2-1");
        }

        [Test]
        public async Task When_Getting_Framework_And_Response_StatusCode_Is_Success_Then_Return_Viewmodel()
        {

            var result = await _sut.GetFramework("230-2-1");

            result.Should().BeOfType<FrameworkDetailsViewModel>();
        }

        [Test]
        public async Task When_Getting_Framework_And_Response_StatusCode_Is_Success_Then_Map_Results()
        {

            var result = await _sut.GetFramework("230-2-1");

            _frameworkMapperMock.Verify(v => v.Map(It.IsAny<Framework>()),Times.Once);
        }
    }
}
