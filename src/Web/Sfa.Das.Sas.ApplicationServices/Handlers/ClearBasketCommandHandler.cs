using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sfa.Das.Sas.ApplicationServices.Commands;
using Sfa.Das.Sas.Shared.Basket.Interfaces;
using Sfa.Das.Sas.Shared.Basket.Models;

namespace Sfa.Das.Sas.ApplicationServices.Handlers
{
    
    public class ClearBasketCommandHandler : IRequestHandler<ClearBasketCommand, Guid>
    {

        private readonly IApprenticeshipFavouritesBasketStore _basketStore;

        public ClearBasketCommandHandler(IApprenticeshipFavouritesBasketStore basketStore)
        {
            _basketStore = basketStore;
        }

        public async Task<Guid> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = await GetBasket(request);

            if (basket != null)
            {
               await _basketStore.ClearBasketAsync(basket);
            }

            return Guid.Empty;
        }

        private async Task<ApprenticeshipFavouritesBasket> GetBasket(ClearBasketCommand request)
        {
            if (request.BasketId.HasValue)
            {
                return await _basketStore.GetAsync(request.BasketId.Value);
            }

            return null;
        }
    }
}
