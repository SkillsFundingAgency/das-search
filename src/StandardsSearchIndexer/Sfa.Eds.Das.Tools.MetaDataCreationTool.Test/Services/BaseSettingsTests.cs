namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test.Services
{
    using System.Configuration;

    using NUnit.Framework;
    using MetaDataCreationTool.Services;

    [TestFixture]
    public class BaseSettingsTests
    {
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
            var settings = new Settings();
            settings.GetSetting("FailToGetSetting");
        }

        [Test]
        public void GettingSetting()
        {
            var settings = new Settings();
            var setting = settings.GetSetting("intAsString");
            Assert.AreEqual("five", setting);
        }

        // Helper
        private string VstsGitFolderPath => new BaseSettings().GetSetting();

        private string VstsGitFolderPathFail => new BaseSettings().GetSetting();

    }
}