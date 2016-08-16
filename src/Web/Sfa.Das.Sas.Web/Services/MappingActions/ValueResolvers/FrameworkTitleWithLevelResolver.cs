namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using AutoMapper;

    using Sfa.Das.Sas.ApplicationServices.Responses;
    using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

    public class FrameworkTitleWithLevelResolver : ValueResolver<GetFrameworkProvidersResponse, string>
    {
        protected override string ResolveCore(GetFrameworkProvidersResponse source)
        {
            var title = ApprenticeshipMappingHelper.FrameworkTitle(source.Title);

            return $"{title}, level {source.Level}";
        }
    }
}