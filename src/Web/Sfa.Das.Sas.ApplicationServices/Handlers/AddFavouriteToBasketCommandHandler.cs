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
            bool basketChanged;
            var basket = await GetBasket(request);

            if (basket == null)
            {
                basketChanged = true;
                basket = CreateNewBasket(request);
            }
            else
            {
                if (request.Ukprn.HasValue)
                {
                    basketChanged = basket.Add(request.ApprenticeshipId, request.Ukprn.Value);
                }
                else
                {
                    basketChanged = basket.Add(request.ApprenticeshipId);
                }
            }

            if (basketChanged)
            {
                await _basketStore.UpdateAsync(basket);
            }

            _logger.LogDebug("Updated apprenticeship basket: {basketId}", basket.Id);

            return basket.Id;
        }

        private static ApprenticeshipFavouritesBasket CreateNewBasket(AddFavouriteToBasketCommand request)
        {
            var basket = new ApprenticeshipFavouritesBasket();

            if (request.Ukprn.HasValue)
            {
                basket.Add(request.ApprenticeshipId, request.Ukprn.Value);
            }
            else
            {
                basket.Add(request.ApprenticeshipId);
            }

            return basket;
        }

        private async Task<ApprenticeshipFavouritesBasket> GetBasket(AddFavouriteToBasketCommand request)
        {
            if (request.BasketId.HasValue)
            {
                return await _basketStore.GetAsync(request.BasketId.Value);
            }

            return null;
        }
    }
}
