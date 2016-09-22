using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Infrastructure.Services;

namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.Services
{
    using Sfa.Das.Sas.Indexer.Core.Logging;

    [TestFixture]
    public class VstsClientTests
    {
        [Test]
        public void ShouldDownloadAString()
        {
            var settings = Mock.Of<IAppServiceSettings>(x => x.VstsGitGetFilesUrlFormat == "{0}");
            var mockHttp = new Mock<IHttpGet>();

            var sut = new VstsClient(settings, mockHttp.Object, Mock.Of<IHttpPost>(), Mock.Of<ILog>());

            sut.GetFileContent("some/path");

            mockHttp.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}