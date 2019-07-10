using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices.Commands;
using System;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class BasketController : Controller
    {
        private readonly IMediator _mediator;
        private const string BasketCookieName = "ApprenticeshipBasket";

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(string apprenticeshipId, int? ukprn)
        {
            // Validate arg formats
            //Fail: throw exception

            // Get cookie
            var cookie = Request.Cookies[BasketCookieName];
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            // Merge item into basket
            var command = new AddFavouriteToBasketCommand
            {
                ApprenticeshipId = apprenticeshipId,
                BasketId = cookieBasketId
            };

            var basketId = await _mediator.Send(command);

            if (cookie == null)
                Response.Cookies.Append(BasketCookieName, basketId.ToString());

            return Redirect("https://google.com");
        }
    }
}
