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
            MetaData metaData = new MetaData();
            metaData.GenerateStandardMetadataFiles();
        }
    }
}
