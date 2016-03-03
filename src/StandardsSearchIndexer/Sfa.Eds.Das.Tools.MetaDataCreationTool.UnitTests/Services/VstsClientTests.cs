namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test.Services
{
    using Moq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Helper;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    [TestFixture]
    public class VstsClientTests
    {
        [Test]
        public void ShouldDownloadAString()
        {
            var settings = Mock.Of<ISettings>(x => x.VstsGitGetFilesUrlFormat == "{0}");
            var mockHttp = new Mock<IHttpHelper>();

            var sut = new VstsClient(settings, mockHttp.Object);

            sut.GetFileContent("some/path");

            mockHttp.Verify(x => x.DownloadString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}