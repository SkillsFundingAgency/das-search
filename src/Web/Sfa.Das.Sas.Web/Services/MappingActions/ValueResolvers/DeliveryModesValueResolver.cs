using System.Collections.Generic;
using AutoMapper;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    public class DeliveryModesValueResolver : IValueResolver<BaseProviderSearchResults, object, IEnumerable<DeliveryModeViewModel>>
    {
        public IEnumerable<DeliveryModeViewModel> Resolve(BaseProviderSearchResults source, object destination, IEnumerable<DeliveryModeViewModel> destMember, ResolutionContext context)
        {
            if (source == null)
            {
                return new List<DeliveryModeViewModel>();
            }

            return ProviderSearchMappingHelper.CreateDeliveryModes(source.TrainingOptionsAggregation, source.SelectedTrainingOptions);
        }
    }
}