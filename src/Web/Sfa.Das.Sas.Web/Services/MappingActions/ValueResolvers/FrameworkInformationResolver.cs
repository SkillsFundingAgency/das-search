namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using AutoMapper;

    using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

    public class FrameworkInformationResolver : ValueResolver<string, string>
    {
        protected override string ResolveCore(string information)
        {
            return ApprenticeshipMappingHelper.GetInformationText(information);
        }
    }
}