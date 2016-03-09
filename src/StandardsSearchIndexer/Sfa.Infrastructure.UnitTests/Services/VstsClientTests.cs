namespace Sfa.Infrastructure.UnitTests.Services
{
    using Moq;

    using NUnit.Framework;

    using Sfa.Deds.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Http;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services.Interfaces;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;

    [TestFixture]
    public class VstsClientTests
    {
        [Test]
        public void ShouldDownloadAString()
        {
            var settings = Mock.Of<IAppServiceSettings>(x => x.VstsGitGetFilesUrlFormat == "{0}");
            var mockHttp = new Mock<IHttpGet>();

            var sut = new VstsClient(settings, mockHttp.Object);

            sut.GetFileContent("some/path");

            mockHttp.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}