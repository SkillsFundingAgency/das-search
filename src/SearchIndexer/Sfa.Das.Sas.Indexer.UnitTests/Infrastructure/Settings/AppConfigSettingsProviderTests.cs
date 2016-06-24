using System.Configuration;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Settings;

namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.Settings
{
    [TestFixture]
    public sealed class AppConfigSettingsProviderTests
    {
        private AppConfigSettingsProvider _appConfigSettings;
        private Mock<IProvideSettings> _mockProviderSettings;

        [SetUp]
        public void Setup()
        {
            _mockProviderSettings = new Mock<IProvideSettings>();
            _appConfigSettings = new AppConfigSettingsProvider(_mockProviderSettings.Object);
        }

        [Test]
        public void TryGetMissingSetting()
        {
            Assert.Throws<ConfigurationErrorsException>(() => _appConfigSettings.GetSetting("FailToGetSetting"))
                  .Message.Should().Be("Setting with key FailToGetSetting is missing");
        }

        [Test]
        public void GettingSetting()
        {
            var setting = _appConfigSettings.GetSetting("intAsString");
            Assert.AreEqual("five", setting);
        }

        [Test]
        public void ShouldTryBaseSettingsProviderIfNoSettingExistsInConfig()
        {
            _mockProviderSettings.Setup(x => x.GetSetting(It.IsAny<string>())).Returns("FromTestSettingsProvider");

            var setting = _appConfigSettings.GetSetting("DoesNotExistInAppConfig");

            Assert.That(setting, Is.EqualTo("FromTestSettingsProvider"));
        }

        [Test]
        public void ShouldTryBaseSettingsProviderIfSettingIsEmptyInConfig()
        {
            _mockProviderSettings.Setup(x => x.GetSetting(It.IsAny<string>())).Returns("FromTestSettingsProvider");

            var setting = _appConfigSettings.GetSetting("EmptySetting");

            Assert.That(setting, Is.EqualTo("FromTestSettingsProvider"));
        }
    }
}
