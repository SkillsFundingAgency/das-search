namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services.Mapping
{
    using Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers;

    public class OverallCohortResolverTestClass : OverallCohortResolver
    {
        public string TestResolveCore(string str)
        {
            return this.ResolveCore(str);
        }
    }
}