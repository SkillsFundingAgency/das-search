using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Logging;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Settings;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.UnitTests
{
    using System;

    using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;

    [TestFixture]
    public class VstsTest
    {
        private IAppServiceSettings _appServiceSettings;

        private string AllIdsResponse
            =>
                "{\"count\":5,\"value\":[{\"objectId\":\"2327ac84367cf719cacdbd46f423be6c2495e70c\",\"gitObjectType\":\"tree\",\"commitId\":\"e58fddc4b8ffe494bba6dedcefb40991d141e273\",\"path\":\"/standards/local/json\",\"isFolder\":true,\"url\":\"https://sfa-gov-uk.visualstudio.com/DefaultCollection/Digital%20Apprenticeship%20Service/_apis/git/repositories/9b4f676e-ce9a-4f10a0430ec9e5bf053citems/standards/local/json?versionType=Branch&versionOptions=None\"},{\"objectId\":\"b64eeb1a2f7af4cb26470c6496e0b254106f86ff\",\"gitObjectType\":\"blob\",\"commitId\":\"e58fddc4b8ffe494bba6dedcefb40991d141e273\",\"path\":\"/standards/local/json/1-NetworkEngineer.json\",\"url\":\"https://sfa-gov-uk.visualstudio.com/DefaultCollection/Digital%20Apprenticeship%20Service/_apis/git/repositories/9b4f676e-ce9a-4f10-a043-0ec9e5bf053c/items/standards/local/json/1-NetworkEngineer.json?versionType=Branch&versionOptions=None\"},{\"objectId\":\"554c08820dcde3b1a872ba86e55dec881e0adda9\",\"gitObjectType\":\"blob\",\"commitId\":\"e58fddc4b8ffe494bba6dedcefb40991d141e273\",\"path\":\"/standards/local/json/10-Electrical_ElectronicTechnicalSupportEngineer.json\",\"url\":\"https://sfa-gov-uk.visualstudio.com/DefaultCollection/Digital%20Apprenticeship%20Service/_apis/git/repositories/9b4f676e-ce9a-4f10a0430ec9e5bf053citems/standards/local/json/10-Electrical_ElectronicTechnicalSupportEngineer.json?versionType=Branch&versionOptions=None\"},{\"objectId\":\"c9987b342bde74297999bed0a305d6171a4cf9e0\",\"gitObjectType\":\"blob\",\"commitId\":\"e58fddc4b8ffe494bba6dedcefb40991d141e273\",\"path\":\"/standards/local/json/11-ManufacturingEngineer.json\",\"url\":\"https://sfa-gov-uk.visualstudio.com/DefaultCollection/Digital%20Apprenticeship%20Service/_apis/git/repositories/9b4f676e-ce9a-4f10a0430ec9e5bf053citems/standards/local/json/11-ManufacturingEngineer.json?versionType=Branch&versionOptions=None\"},{\"objectId\":\"2ecba714335a2ede11d59b2835b8776fc80e0e0c\",\"gitObjectType\":\"blob\",\"commitId\":\"e58fddc4b8ffe494bba6dedcefb40991d141e273\",\"path\":\"/standards/local/json/12-ProductDesignandDevelopmentEngineer.json\",\"url\":\"https://sfa-gov-uk.visualstudio.com/DefaultCollection/Digital%20Apprenticeship%20Service/_apis/git/repositories/9b4f676e-ce9a-4f10a0430ec9e5bf053citems/standards/local/json/12-ProductDesignandDevelopmentEngineer.json?versionType=Branch&versionOptions=None\"},{\"objectId\":\"3c29d43cef4e555bf9bec3e6ac64be917ecbbd63\",\"gitObjectType\":\"blob\",\"commitId\":\"e58fddc4b8ffe494bba6dedcefb40991d141e273\",\"path\":\"/standards/local/json/13-ProductDesignandDevelopmentTechnician.json\",\"url\":\"https://sfa-gov-uk.visualstudio.com/DefaultCollection/Digital%20Apprenticeship%20Service/_apis/git/repositories/9b4f676e-ce9a-4f10-a043-0ec9e5bf053citems/standards/local/json/13-ProductDesignandDevelopmentTechnician.json?versionType=Branch&versionOptions=None\"}]}";

        [SetUp]
        public void Setup()
        {
            var appConfigSettingsProvider = new Mock<IProvideSettings>();
            appConfigSettingsProvider.Setup(x => x.GetSetting("GitUsername")).Returns("username");
            appConfigSettingsProvider.Setup(x => x.GetSetting("GitPassword")).Returns("pasword");
            _appServiceSettings = new AppServiceSettings(appConfigSettingsProvider.Object);
        }

        [Test]
        [Category("ExternalDependency")]
        public void GeStandards()
        {
            var httpHelperMock = new Mock<IVstsClient>();
            var mockLogger = new Mock<ILog>(MockBehavior.Loose);

            httpHelperMock.Setup(m => m.Get(It.IsAny<string>())).Returns(AllIdsResponse);

            var vsts = new VstsService(_appServiceSettings, new GitDynamicModelGenerator(), new JsonMetaDataConvert(null), httpHelperMock.Object, mockLogger.Object);
            var standards = vsts.GetStandards();
            Assert.AreEqual(5, standards.Count());
        }

        [Test]
        public void GetIds()
        {
            var httpHelperMock = new Mock<IVstsClient>();
            var mockLogger = new Mock<ILog>(MockBehavior.Loose);

            httpHelperMock.Setup(m => m.Get(It.IsAny<string>())).Returns(AllIdsResponse);
            var vsts = new VstsService(_appServiceSettings, new GitDynamicModelGenerator(), null, httpHelperMock.Object, mockLogger.Object);
            var ids = vsts.GetExistingStandardIds();

            Debug.Assert(ids != null, "ids != null");

            Assert.AreEqual(5, ids.Count());
            Assert.IsTrue(ids.Contains("13"));
            Assert.IsFalse(ids.Contains("14"));
        }

        [Test]
        public void ShouldReturnNewDictionaryIfBlobsAreNull()
        {
            var httpHelperMock = new Mock<IVstsClient>();
            var mockLogger = new Mock<ILog>(MockBehavior.Loose);

            httpHelperMock.Setup(m => m.Get(It.IsAny<string>())).Returns(AllIdsResponse);
            var vsts = new VstsService(_appServiceSettings, new GitDynamicModelGenerator(), null, httpHelperMock.Object, mockLogger.Object);

            httpHelperMock.Setup(m => m.Get(It.IsAny<string>())).Returns(string.Empty);
            var ids = vsts.GetAllFileContents(_appServiceSettings.VstsGitGetFilesUrl);

            Assert.AreEqual(ids, new Dictionary<string, string>());
        }
    }
}