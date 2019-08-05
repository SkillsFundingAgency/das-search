using System;
using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices.Models;

namespace Sfa.Das.Sas.ApplicationServices.Interfaces
{
    public interface IApprenticeshipFavouritesBasketStore
    {
        Task<ApprenticeshipFavouritesBasketRead> GetAsync(Guid basketId);
        Task UpdateAsync(Guid basketId, ApprenticeshipFavouritesBasketWrite basket);
    }
}
