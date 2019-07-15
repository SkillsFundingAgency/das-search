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
    public class AddFavouriteToBasketCommandHandler : IRequestHandler<AddFavouriteToBasketCommand, Guid>
    {
        private readonly IApprenticeshipFavouritesBasketStore _basketStore;

        public AddFavouriteToBasketCommandHandler(IApprenticeshipFavouritesBasketStore basketStore)
        {
            _basketStore = basketStore;
        }

        public async Task<Guid> Handle(AddFavouriteToBasketCommand request, CancellationToken cancellationToken)
        {
            ApprenticeshipFavouritesBasket basket;
            Guid basketId;

            if (request.BasketId.HasValue)
            {
                basketId = request.BasketId.Value;
                basket = await _basketStore.GetAsync(request.BasketId.Value);

                if (basket == null)
                {
                    CreateNewBasket(request, out basket, out basketId);
                }
                else
                {
                    if (basket.Any(x => x.ApprenticeshipId == request.ApprenticeshipId))
                        return basketId; // Ignore if saving just an apprenticehip that is already in the basket.

                    basket.Add(new ApprenticeshipFavourite(request.ApprenticeshipId));
                }
            }
            else
            {
                CreateNewBasket(request, out basket, out basketId);
            }

            await _basketStore.UpdateAsync(basketId, basket);

            return basketId;
        }

        private static void CreateNewBasket(AddFavouriteToBasketCommand request, out ApprenticeshipFavouritesBasket basket, out Guid basketId)
        {
            basketId = Guid.NewGuid();
            basket = new ApprenticeshipFavouritesBasket
                {
                    new ApprenticeshipFavourite(request.ApprenticeshipId)
                };
        }
    }
}
