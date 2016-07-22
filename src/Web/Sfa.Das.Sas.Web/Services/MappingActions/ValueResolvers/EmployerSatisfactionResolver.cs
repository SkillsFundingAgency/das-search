namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using AutoMapper;

    using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

    public class EmployerSatisfactionResolver : ValueResolver<double?, string>
    {
        protected override string ResolveCore(double? satisfaction)
        {
            return ProviderMappingHelper.GetPercentageText(satisfaction);
        }
    }
}