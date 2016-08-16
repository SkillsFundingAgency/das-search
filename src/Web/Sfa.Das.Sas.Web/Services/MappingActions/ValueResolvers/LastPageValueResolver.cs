namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using ApplicationServices.Models;
    using AutoMapper;
    using Helpers;

    public class LastPageValueResolver : ValueResolver<BaseProviderSearchResults, int>
    {
        protected override int ResolveCore(BaseProviderSearchResults source)
        {
            if (source == null)
            {
                return 0;
            }

            return SearchMappingHelper.CalculateLastPage(source.TotalResults, source.ResultsToTake);
        }
    }
}