namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    using StructureMap;
    public static class ContainerBootstrapper
    {
        public static Container BootstrapStructureMap()
        {
            // Initialize the static ObjectFactory container
            var container = new Container(
                c =>
                    {
                        c.For<ISettings>().Use<Settings>();
                        c.For<ILarsDataService>().Use<LarsDataService>();
                        c.For<ICsvService>().Use<CsvService>();
                        c.For<IAngleSharpService>().Use<AngleSharpService>();
                        c.For<IVstsService>().Use<VstsService>();
                        c.For<ILog4NetLogger>().Use<Log4NetLogger>();
                    });
            return container;
        }
    }
}