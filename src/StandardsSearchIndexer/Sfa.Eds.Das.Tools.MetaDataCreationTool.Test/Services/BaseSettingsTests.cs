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
        public void GetInteger()
        {
            var settings = new Settings(new Log4NetLogger());
            var int1 = settings.GetSetting("int", 2);
            Assert.AreEqual(5, int1);

            var int2 = settings.GetSetting("intAsString", 2);
            Assert.AreEqual(2, int2);
        }

        [Test]
        [ExpectedException(typeof(ConfigurationErrorsException), ExpectedMessage = "Setting with key FailToGetSetting is missing")]
        public void TryGetMissingSetting()
        {
            var settings = new Settings(new Log4NetLogger());
            settings.GetSetting("FailToGetSetting");
        }

        [Test]
        public void GettingSetting()
        {
            var settings = new Settings(new Log4NetLogger());
            var setting = settings.GetSetting("intAsString");
            Assert.AreEqual("five", setting);
        }

        // Helper
        private string VstsGitFolderPath => new BaseSettings().GetSetting();

        private string VstsGitFolderPathFail => new BaseSettings().GetSetting();

    }
}