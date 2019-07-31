using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.Shared.Basket.Interfaces;
using Sfa.Das.Sas.Shared.Basket.Models;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class AddFavouriteToBasketCommandHandler : IRequestHandler<AddFavouriteToBasketCommand, Guid>
    {
        private readonly IApprenticeshipFavouritesBasketStore _basketStore;
        private readonly ILogger<AddFavouriteToBasketCommandHandler> _logger;

        public AddFavouriteToBasketCommandHandler(ILogger<AddFavouriteToBasketCommandHandler> logger, IApprenticeshipFavouritesBasketStore basketStore)
        {
            _basketStore = basketStore;
            _logger = logger;
        }

        public async Task<Guid> Handle(AddFavouriteToBasketCommand request, CancellationToken cancellationToken)
        {
            ApprenticeshipFavouritesBasket basket;
            Guid basketId;

            if (request.BasketId.HasValue)
            {
                basketId = request.BasketId.Value;
                basket = await _basketStore.GetAsync(request.BasketId.Value) ?? new ApprenticeshipFavouritesBasket();

                if (basket.Any(x => x.ApprenticeshipId == request.ApprenticeshipId))
                {
                    if (request.Ukprn.HasValue)
                    {
                        var apprenticeship = basket.First(x => x.ApprenticeshipId == request.ApprenticeshipId);

                        if (apprenticeship.Ukprns.Contains(request.Ukprn.Value))
                        {
                            return basketId;
                        }
                        else
                        {
                            apprenticeship.Ukprns.Add(request.Ukprn.Value);
                        }
                    }
                    else
                    {
                        return basketId; // Ignore if saving just an apprenticehip that is already in the basket.
                    }
                }
                else
                {
                    if (request.Ukprn.HasValue)
                    {
                        basket.Add(new ApprenticeshipFavourite(request.ApprenticeshipId, request.Ukprn.Value));
                    }
                    else
                    {
                        basket.Add(new ApprenticeshipFavourite(request.ApprenticeshipId));
                    }
                }
            }
            else
            {
                CreateNewBasket(request, out basket, out basketId);
            }

            await _basketStore.UpdateAsync(basketId, basket);

            _logger.LogDebug("Updated apprenticeship basket: {basketId}", basketId);

            return basketId;
        }

        private static void CreateNewBasket(AddFavouriteToBasketCommand request, out ApprenticeshipFavouritesBasket basket, out Guid basketId)
        {
            basketId = Guid.NewGuid();
            basket = new ApprenticeshipFavouritesBasket();

            if (request.Ukprn.HasValue)
            {
                basket.Add(new ApprenticeshipFavourite(request.ApprenticeshipId, request.Ukprn.Value));
            }
            else
            {
                basket.Add(new ApprenticeshipFavourite(request.ApprenticeshipId));
            }
        }
    }
}
