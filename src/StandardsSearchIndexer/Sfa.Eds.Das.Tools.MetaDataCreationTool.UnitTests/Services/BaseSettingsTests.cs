namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.UnitTests.Services
{
    using System;
    using System.Configuration;
    using Moq;
    using NUnit.Framework;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;

    [TestFixture]
    public class BaseSettingsTests
    {
        // Helper
        private string VstsGitFolderPath => new BaseSettings().GetSetting();

        private string VstsGitFolderPathFail => new BaseSettings().GetSetting();

        [Test]
        public void GetNameFromCallerMethod()
        {
            var vstsfolder = VstsGitFolderPath;
            Assert.AreEqual("standards/local/json", vstsfolder);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "Setting with key VstsGitFolderPathFail is missing")]
        public void GetNameFromCallerFail()
        {
            // ReSharper disable once UnusedVariable
            var vstsfolder = VstsGitFolderPathFail;
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "Setting with key FailToGetSetting is missing")]
        public void TryGetMissingSetting()
        {
            var settings = new AppServiceSettings();
            settings.GetSetting("FailToGetSetting");
        }

        [Test]
        public void GettingSetting()
        {
            var settings = new AppServiceSettings();
            var setting = settings.GetSetting("intAsString");
            Assert.AreEqual("five", setting);
        }

        [TestCase(typeof(IMaintainApprenticeshipIndex), "Standard.QueueName")]
        [TestCase(typeof(IMaintainProviderIndex), "Provider.QueueName")]
        public void CreateQueueNameFromType(Type type, string queueName)
        {
            var settings = new Mock<AppServiceSettings>();
            settings.Setup(m => m.GetSetting(It.IsAny<string>())).Returns(string.Empty);
            settings.Object.QueueName(type);

            settings.Verify(m => m.GetSetting(queueName), Times.Once);
        }
    }
}