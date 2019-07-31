using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels.Basket;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public interface IBasketViewModelMapper
    {
        BasketViewModel Map(ApprenticeshipFavouritesBasket item);
    }
}
