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
    public class AddorRemoveFavouriteInBasketCommandHandler : IRequestHandler<AddOrRemoveFavouriteInBasketCommand, Guid>
    {
        private readonly IApprenticeshipFavouritesBasketStore _basketStore;
        private readonly ILogger<AddorRemoveFavouriteInBasketCommandHandler> _logger;

        public AddorRemoveFavouriteInBasketCommandHandler(ILogger<AddorRemoveFavouriteInBasketCommandHandler> logger, IApprenticeshipFavouritesBasketStore basketStore)
        {
            _basketStore = basketStore;
            _logger = logger;
        }

        public async Task<Guid> Handle(AddOrRemoveFavouriteInBasketCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.ApprenticeshipId))
            {
                var message = $"Apprenticeship id must be provided for {nameof(AddorRemoveFavouriteInBasketCommandHandler)}";
                _logger.LogWarning(message);
                throw new ArgumentException(message);
            }

            bool basketChanged;
            var basket = await GetBasket(request);

            if (basket == null)
            {
                basketChanged = true;
                basket = CreateNewBasket(request);
            }
            else
            {
                if (request.Ukprn.HasValue && request.LocationId.HasValue)
                {
                    if (basket.IsInBasket(request.ApprenticeshipId, request.Ukprn.Value, request.LocationId.Value))
                    {
                        basket.Remove(request.ApprenticeshipId, request.Ukprn.Value, request.LocationId.Value);
                        basketChanged = true;
                    }
                    else
                    {
                        basketChanged = basket.Add(request.ApprenticeshipId, request.Ukprn.Value, request.LocationId.Value);
                    }
                }
                else if (request.Ukprn.HasValue)
                {
                    if (basket.IsInBasket(request.ApprenticeshipId, request.Ukprn.Value))
                    {
                        basket.Remove(request.ApprenticeshipId, request.Ukprn.Value);
                        basketChanged = true;
                    }
                    else
                    {
                        basketChanged = basket.Add(request.ApprenticeshipId, request.Ukprn.Value);
                    }
                }
                else
                {
                    if (basket.IsInBasket(request.ApprenticeshipId))
                    {
                        basket.Remove(request.ApprenticeshipId);
                        basketChanged = true;
                    }
                    else
                    {
                        basketChanged = basket.Add(request.ApprenticeshipId);
                    }
                }
            }

            if (basketChanged)
            {
                await _basketStore.UpdateAsync(basket);
            }

            _logger.LogDebug("Updated apprenticeship basket: {basketId}", basket.Id);

            return basket.Id;
        }

        private static ApprenticeshipFavouritesBasket CreateNewBasket(AddOrRemoveFavouriteInBasketCommand request)
        {
            var basket = new ApprenticeshipFavouritesBasket();

            if (request.Ukprn.HasValue && request.LocationId.HasValue)
            {
                basket.Add(request.ApprenticeshipId, request.Ukprn.Value, request.LocationId.Value);

            }
            else if (request.Ukprn.HasValue)
            {
                basket.Add(request.ApprenticeshipId, request.Ukprn.Value);
            }
            else
            {
                basket.Add(request.ApprenticeshipId);
            }

            return basket;
        }

        private async Task<ApprenticeshipFavouritesBasket> GetBasket(AddOrRemoveFavouriteInBasketCommand request)
        {
            if (request.BasketId.HasValue)
            {
                return await _basketStore.GetAsync(request.BasketId.Value);
            }

            return null;
        }
    }
}
