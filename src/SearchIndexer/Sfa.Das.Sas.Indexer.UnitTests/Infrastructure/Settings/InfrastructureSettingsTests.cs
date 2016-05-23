using System.Linq;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.Core.Services;
using Sfa.Das.Sas.Indexer.Infrastructure.Settings;

namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.Settings
{
    [TestFixture]
    public sealed class InfrastructureSettingsTests
    {
        [TestCase("http://40.2.2.20:9200,http://world.com", 2, "http://40.2.2.20:9200/")]
        public void GetElasticIPsTest(string settingsResturn, int count, string first)
        {
            var settingsProvider = new Mock<IProvideSettings>();
            settingsProvider.Setup(m => m.GetSetting("ElasticServerUrls")).Returns(settingsResturn);
            var infrastructureSettings = new InfrastructureSettings(settingsProvider.Object);
            var urls = infrastructureSettings.GetElasticIPs("ElasticServerUrls");

            Assert.NotNull(urls);
            Assert.AreEqual(count, urls.Count());
            Assert.AreEqual(first, urls.First().AbsoluteUri);
        }
    }
}
