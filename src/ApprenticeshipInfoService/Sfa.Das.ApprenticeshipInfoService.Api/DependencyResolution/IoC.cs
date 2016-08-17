namespace Sfa.Das.ApprenticeshipInfoService.Api.DependencyResolution
{
    using Infrastructure.DependencyResolution;
    using StructureMap;

    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
                c.AddRegistry<ApiRegistry>();
                c.AddRegistry<InfrastructureRegistry>();
            });
        }
    }
}