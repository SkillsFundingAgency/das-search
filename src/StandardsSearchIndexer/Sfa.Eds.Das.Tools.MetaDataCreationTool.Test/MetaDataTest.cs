using StructureMap;

namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test
{
    using DependencyResolution;
    using MetaDataCreationTool.Services.Interfaces;
    using NUnit.Framework;

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
            var settings = container.GetInstance<ISettings>();

            // Set GitUserName/GitPassword in app.config to run this
            MetaDataManager metaData = new MetaDataManager(larsDataService, vstsDataService, settings);

            metaData.GenerateStandardMetadataFiles();
        }
    }
}
