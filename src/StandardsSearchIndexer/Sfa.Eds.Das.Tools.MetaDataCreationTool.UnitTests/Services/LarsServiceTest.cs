namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Test.Services
{
    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;
    using Sfa.Infrastructure.Services;

    using CsvService = Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.CsvService;

    [TestFixture]
    public class LarsServiceTest
    {
        [Test]
        [Category("Integration")]
        [Category("Nightly")]
        public void TestGetFrameworksFromZipFile()
        {
            var zipFIleExtractor = new ZipFileExtractor();
            var csvService = new CsvService(null, null);
            var applicationSettings = new AppServiceSettings();
            LarsDataService larsDataService = new LarsDataService(applicationSettings, csvService, new HttpService(null), zipFIleExtractor, new AngleSharpService(), null, new HttpService(null));
            var path = larsDataService.GetListOfCurrentFrameworks();

            Assert.IsTrue(path.Count > 100);
        }
    }
}
