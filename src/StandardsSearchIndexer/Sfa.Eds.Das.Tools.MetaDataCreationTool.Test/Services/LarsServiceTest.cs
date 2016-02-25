namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test
{
    using NUnit.Framework;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;

    [TestFixture]
    public class LarsServiceTest
    {
        [Test]
        [Category("ExternalDependency")]
        public void GetZipFileFromGovLearn()
        {
            var larsDataService = new LarsDataService(new Settings(), null, new HttpHelper());
            var path = larsDataService.GetZipFilePath();
            Assert.True(path.StartsWith("https://hub.imservices.org.uk/Learning%20Aims/Downloads/Documents/"));
        }

    }
}
