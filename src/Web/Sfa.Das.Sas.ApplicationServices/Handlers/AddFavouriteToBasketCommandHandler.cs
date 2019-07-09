using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.ApplicationServices.Interfaces;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class AddFavouriteToBasketCommandHandler : AsyncRequestHandler<AddFavouriteToBasketCommand>
    {
        private readonly IFavouritesBasketStore _basketStore;

        public AddFavouriteToBasketCommandHandler(IFavouritesBasketStore basketStore)
        {
            _basketStore = basketStore;
        }

        protected override async Task Handle(AddFavouriteToBasketCommand request, CancellationToken cancellationToken)
        {
            ApprenticeshipFavouritesBasket basket;
            Guid basketId;

            if (request.BasketId.HasValue)
            {
                basketId = request.BasketId.Value;
                basket = await _basketStore.GetAsync(request.BasketId.Value);

                if (basket.Any(x => x.ApprenticeshipId == request.ApprenticeshipId))
                    return; // Ignore if saving just an apprenticehip that is already in the basket.

                basket.Add(new ApprenticeshipFavourite(request.ApprenticeshipId));
            }
            else
            {
                basketId = Guid.NewGuid();
                basket = new ApprenticeshipFavouritesBasket
                {
                    new ApprenticeshipFavourite(request.ApprenticeshipId)
                };
            }

            await _basketStore.UpdateAsync(basketId, basket);
        }
    }
}
