namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using AutoMapper;
    using Sfa.Das.Sas.ApplicationServices.Models;

    public class LastPageValueResolver : ValueResolver<BaseProviderSearchResults, int>
    {
        protected override int ResolveCore(BaseProviderSearchResults source)
        {
            if (source == null)
            {
                return 0;
            }

            return ProviderSearchMappingHelper.CalculateLastPage(source.TotalResults, source.ResultsToTake);
        }
    }
}