using System;
using MediatR;
using Sfa.Das.Sas.Shared.Basket.Models;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public class GetBasketQuery : IRequest<ApprenticeshipFavouritesBasket>
    {
        public Guid BasketId { get; set; }
    }
}