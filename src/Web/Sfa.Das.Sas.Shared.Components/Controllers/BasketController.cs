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
        public async Task<IActionResult> Add(string selectedItem)
        {
            // Validate arg formats
            //Fail: throw exception
            var parts = selectedItem.Split('$');
            var apprenticeshipId = parts[0];
            var ukprn = parts[1] == string.Empty ? null : parts[1];

            // Get cookie
            var cookie = Request.Cookies[BasketCookieName];
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            var basketId = await _mediator.Send(new AddFavouriteToBasketCommand
            {
                ApprenticeshipId = apprenticeshipId,
                BasketId = cookieBasketId
            });

            if (cookie == null)
                Response.Cookies.Append(BasketCookieName, basketId.ToString());

            return Redirect("https://google.com");
        }
    }
}
