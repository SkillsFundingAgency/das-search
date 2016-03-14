using StructureMap;

namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test
{
    using DependencyResolution;
    using Indexer.Core.Services;
    using MetaDataCreationTool.Services.Interfaces;
    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.MetaData;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;

    [TestFixture]
    public class MetaDataTest
    {
        [Test]
        [Category("ExternalDependency")]
        [Ignore("Integration run")]
        public void TestGenerationOfFiles()
        {
            var container = new Container(c =>
            {
                c.AddRegistry<MetaDataCreationRegistry>();
            });

            var larsDataService = container.GetInstance<ILarsDataService>();
            var vstsDataService = container.GetInstance<IVstsService>();
            var settings = container.GetInstance<IAppServiceSettings>();
            var logger = container.GetInstance<ILog>();

            // Set GitUserName/GitPassword in app.config to run this
            MetaDataManager metaData = new MetaDataManager(larsDataService, vstsDataService, settings, logger);

            metaData.GenerateStandardMetadataFiles();
        }
    }
}
