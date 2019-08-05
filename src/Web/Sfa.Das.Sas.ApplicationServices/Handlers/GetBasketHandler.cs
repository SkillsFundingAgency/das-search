using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Basket.Interfaces;
using Sfa.Das.Sas.Shared.Basket.Models;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    public class GetBasketHandler : IRequestHandler<GetBasketQuery, ApprenticeshipFavouritesBasket>
    {
        private readonly ILogger<GetBasketHandler> _logger;
        private readonly IApprenticeshipFavouritesBasketStore _basketStore;

         public GetBasketHandler(
            ILogger<GetBasketHandler> logger,
            IApprenticeshipFavouritesBasketStore basketStore)
        {
            _logger = logger;
            _basketStore = basketStore;
        }

        public async Task<ApprenticeshipFavouritesBasket> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting basket for {basketId}", request.BasketId);

            var basket = await _basketStore.GetAsync(request.BasketId);

            return basket ?? new ApprenticeshipFavouritesBasket();
        }
    }
}
