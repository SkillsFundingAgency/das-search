using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure;
using Sfa.Das.Sas.Indexer.ApplicationServices.MetaData;
using Sfa.Das.Sas.Indexer.ApplicationServices.Provider;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;
using Sfa.Das.Sas.Indexer.Core.Services;

namespace Sfa.Das.Sas.Indexer.UnitTests.ApplicationServices.Provider
{
    [TestFixture]
    public class FcsActiveProvidersClientTest
    {
        [Test]
        public void ShouldGetFcsActiveProviders()
        {
            var moqVstsClient = new Mock<IVstsClient>();
            var moqIProvideSettings = new Mock<IProvideSettings>();
            var appsettings = new AppServiceSettings(moqIProvideSettings.Object);
            var moqIConvertFromCsv = new Mock<IConvertFromCsv>();

            moqVstsClient.Setup(m => m.GetFileContent(It.IsAny<string>())).Returns(string.Empty);
            moqIConvertFromCsv.Setup(m => m.CsvTo<ActiveProviderCsvRecord>(It.IsAny<string>())).Returns(new[] { new ActiveProviderCsvRecord { UkPrn = 26 }, new ActiveProviderCsvRecord { UkPrn = 126 } });

            var client = new FcsActiveProvidersClient(moqVstsClient.Object, appsettings, moqIConvertFromCsv.Object);
            var result = client.GetActiveProviders();

            result.Count().Should().Be(2);
            result.All(m => new[] { 26, 126 }.Contains(m)).Should().BeTrue();
        }
    }
}