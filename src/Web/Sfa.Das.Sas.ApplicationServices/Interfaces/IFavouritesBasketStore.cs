using System;
using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.ApplicationServices.Interfaces
{
    public interface IFavouritesBasketStore
    {
        Task<ApprenticeshipFavouritesBasket> GetAsync(Guid basketId);
        Task UpdateAsync(Guid basketId, ApprenticeshipFavouritesBasket basket);
    }
}
