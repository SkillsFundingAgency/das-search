using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Basket
{
    public class BasketIconViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly IBasketOrchestrator _orchestrator;

        public BasketIconViewComponent(IMediator mediator, IBasketOrchestrator orchestrator)
        {
            _mediator = mediator;
            _orchestrator = orchestrator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new BasketIconViewModel
            {
                ItemCount = await GetBasketItemCount(),
                BasketUrl = Url.Link("BasketView", null)
            };

            return View("../Basket/BasketIcon/Default", model);
        }

        private async Task<int> GetBasketItemCount()
        {
            var basket = await _orchestrator.GetBasket();

            return basket.Items?.Count ?? 0;
        }
    }
}
