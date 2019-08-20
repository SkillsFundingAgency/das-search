using System;
using MediatR;

namespace Sfa.Das.Sas.ApplicationServices.Commands
{
    public class AddFavouriteToBasketCommand : IRequest<Guid>
    {
        public Guid? BasketId { get; set; }
        public string ApprenticeshipId { get; set; }
        public int? Ukprn { get; set; }
    }
}
