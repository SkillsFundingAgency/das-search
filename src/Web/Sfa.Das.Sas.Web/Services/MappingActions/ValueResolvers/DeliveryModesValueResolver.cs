using System.Collections.Generic;
using AutoMapper;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    public class DeliveryModesValueResolver : ValueResolver<BaseProviderSearchResults, IEnumerable<DeliveryModeViewModel>>
    {
        protected override IEnumerable<DeliveryModeViewModel> ResolveCore(BaseProviderSearchResults source)
        {
            if (source == null)
            {
                return new List<DeliveryModeViewModel>();
            }

            return ProviderSearchMappingHelper.CreateDeliveryModes(source.TrainingOptionsAggregation, source.SelectedTrainingOptions);
        }
    }
}