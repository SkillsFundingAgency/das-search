namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test
{
    using NUnit.Framework;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;

    [TestFixture]
    public class MetadataCreationTest
    {
        [Test]
        [Category("ExternalDependency")]
        public void GetZipFileFromGovLearn()
        {
            var metacreation = new LarsDataService(new Settings(), null);
            var path = metacreation.GetZipFilePath();
            Assert.True(path.StartsWith("https://hub.imservices.org.uk/Learning%20Aims/Downloads/Documents/"));
        }

    }
}
