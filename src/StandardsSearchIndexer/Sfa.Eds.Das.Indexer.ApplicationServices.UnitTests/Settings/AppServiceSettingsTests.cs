using System;
using Moq;
using NUnit.Framework;
using Sfa.Eds.Das.Indexer.ApplicationServices.Services;
using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
using Sfa.Eds.Das.Indexer.Core.Services;

namespace Sfa.Eds.Das.Indexer.ApplicationServices.UnitTests.Settings
{
    [TestFixture]
    public sealed class AppServiceSettingsTests
    {
        [TestCase(typeof(IMaintainApprenticeshipIndex), "Apprenticeship.QueueName")]
        [TestCase(typeof(IMaintainProviderIndex), "Provider.QueueName")]
        public void CreateQueueNameFromType(Type type, string queueName)
        {
            var settingsProvider = new Mock<IProvideSettings>();
            settingsProvider.Setup(m => m.GetSetting(It.IsAny<string>())).Returns(string.Empty);
            var settings = new AppServiceSettings(settingsProvider.Object);

            settings.QueueName(type);

            settingsProvider.Verify(m => m.GetSetting(queueName), Times.Once);
        }
    }
}
