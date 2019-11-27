using System;
using MediatR;

namespace Sfa.Das.Sas.ApplicationServices.Commands
{
    public class AddOrRemoveFavouriteInBasketCommand : IRequest<Guid>
    {
        public Guid? BasketId { get; set; }
        public string ApprenticeshipId { get; set; }
        public int? Ukprn { get; set; }
        public int? LocationId { get; set; }
    }
}
