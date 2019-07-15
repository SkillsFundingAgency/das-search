using AutoMapper;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using Sfa.Das.Sas.ApplicationServices.Models;

    public class NationalProvidersValueResolver : ValueResolver<BaseProviderSearchResults, NationalProviderViewModel>
    {
        protected override NationalProviderViewModel ResolveCore(BaseProviderSearchResults source)
        {
            if (source == null)
            {
                return new NationalProviderViewModel();
            }

            return ProviderSearchMappingHelper.GetNationalProvidersAmount(source.NationalProviders, source.ShowNationalProvidersOnly);
        }
    }
}