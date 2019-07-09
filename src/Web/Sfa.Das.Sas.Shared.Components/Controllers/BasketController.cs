using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices.Commands;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class BasketController : Controller
    {
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(string apprenticeshipId, int? ukprn)
        {
            // Validate arg formats
            //Fail: throw exception

            // Merge item into basket
            var command = new AddFavouriteToBasketCommand
            {
                ApprenticeshipId = apprenticeshipId,
                BasketId = null
            };

            var basketId = await _mediator.Send(command);

            return Redirect("https://google.com");
        }
    }
}
