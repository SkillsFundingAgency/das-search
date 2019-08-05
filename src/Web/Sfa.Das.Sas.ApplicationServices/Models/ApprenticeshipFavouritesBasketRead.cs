using System.Collections.Generic;
using System.Linq;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class ApprenticeshipFavouritesBasketRead : List<ApprenticeshipFavouriteRead>
    {
        public ApprenticeshipFavouritesBasketWrite ToBasketWrite()
        {
            var basketWrite = new ApprenticeshipFavouritesBasketWrite();

            basketWrite.AddRange(this.Select(s => new ApprenticeshipFavouriteWrite(s.ApprenticeshipId) {Ukprns = s.Providers.Select(t => t.Ukprn).ToList()}).ToList());
            return basketWrite;
        }
    }
}
