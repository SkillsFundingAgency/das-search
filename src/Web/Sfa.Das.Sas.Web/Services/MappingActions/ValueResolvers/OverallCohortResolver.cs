namespace Sfa.Das.Sas.Web.Services.MappingActions.ValueResolvers
{
    using System;

    using AutoMapper;

    public class OverallCohortResolver : ValueResolver<string, string>
    {
        protected override string ResolveCore(string source)
        {
            if (source == null)
            {
                return "*";
            }

            if (source.Equals("-", StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            return source;
        }
    }
}