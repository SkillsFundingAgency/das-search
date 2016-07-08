using System;
using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using AutoMapper;

    using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

    public class DeliveryOptionResolver : ValueResolver<List<string>, string>
    {
        protected override string ResolveCore(List<string> deliveryOptions)
        {
            return ProviderMappingHelper.GetDeliveryOptionText(deliveryOptions);
        }
    }
}