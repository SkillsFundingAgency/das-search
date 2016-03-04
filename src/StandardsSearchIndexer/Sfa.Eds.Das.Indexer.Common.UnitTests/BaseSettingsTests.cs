namespace Sfa.Eds.Das.Indexer.Common.UnitTests
{
    using System.Configuration;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Indexer.Common.Settings;

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

        // Helper
        private string VstsGitFolderPath => new BaseSettings().GetSetting();

        private string VstsGitFolderPathFail => new BaseSettings().GetSetting();

    }
}