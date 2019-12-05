using System;
using MediatR;

namespace Sfa.Das.Sas.ApplicationServices.Commands
{
    public class ClearBasketCommand : IRequest<Guid>
    {
        public Guid? BasketId { get; set; }
    }
}
