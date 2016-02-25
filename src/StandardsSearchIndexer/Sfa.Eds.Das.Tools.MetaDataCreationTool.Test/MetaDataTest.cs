namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test
{
    using NUnit.Framework;

    [TestFixture]    
    public class MetaDataTest
    {
        [Test]
        [Category("ExternalDependency")]
        [Ignore("Integration run")]
        public void TestMethod1()
        {
            // Set GitUserName/GitPassword in app.config to run this
            MetaDataManager metaData = new MetaDataManager();
            metaData.GenerateStandardMetadataFiles();
        }
    }
}
