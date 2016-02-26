using Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper;
using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;
using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;
using StructureMap;

namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.DependencyResolution
{
    public sealed class MetaDataCreationRegistry : Registry
    {
        public MetaDataCreationRegistry()
        {
            For<ISettings>().Use<Settings>();
            For<ILarsDataService>().Use<LarsDataService>();
            For<IReadStandardsFromCsv>().Use<CsvService>();
            For<IAngleSharpService>().Use<AngleSharpService>();
            For<IVstsService>().Use<VstsService>();
            For<ILog4NetLogger>().Use<Log4NetLogger>();
            For<IGitDynamicModelGenerator>().Use<GitDynamicModelGenerator>();
            For<IHttpHelper>().Use<HttpHelper>();
            For<IUnzipFiles>().Use<ZipFileExtractor>();
        }
    }
}
