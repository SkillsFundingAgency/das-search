using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Components.Cookies;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Basket;

namespace Sfa.Das.Sas.Shared.Components.UnitTests.ViewComponents.Basket
{
    [TestFixture]
    public class BasketDetailsViewComponentTests : ViewComponentTestsBase
    {
        private Mock<IBasketOrchestrator> _mockBasketOrchestrator;
        private BasketDetailsViewComponent _sut;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _mockBasketOrchestrator = new Mock<IBasketOrchestrator>();

            _sut = new BasketDetailsViewComponent(_mockBasketOrchestrator.Object)
            {
                ViewComponentContext = _viewComponentContext
            };
        }

        [Test]
        public async Task Invoke_ReturnsModelContainingApprenticeshipId()
        {
            var result = await _sut.InvokeAsync() as ViewViewComponentResult;

        }


    }
}
