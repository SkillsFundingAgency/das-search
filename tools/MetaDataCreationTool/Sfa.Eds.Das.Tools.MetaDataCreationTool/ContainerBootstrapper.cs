namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.IO.Compression;

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
                        c.For<IMetaDataCreation>().Use<MetaDataCreation>();
                        c.For<ICsvService>().Use<CsvService>();
                        c.For<IFileService>().Use<FileService>();
                        c.For<IAngleSharpService>().Use<AngleSharpService>();
                        c.For<IJsonStringFormater>().Use<JsonStringFormater>();
                        c.For<IVstsService>().Use<VstsService>();
                    });
            return container;
        }
    }
}