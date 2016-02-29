namespace UnitTestProject1
{
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services;

    using UnitTestProject1.Helpers;

    [TestClass]
    public class MetaDataTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var angelService = new AngleSharpService();
            var standardsUrl = "https://www.gov.uk/government/collections/apprenticeship-standards";

            var mappings = angelService.GetLinks(standardsUrl, ".publication.document-row h3 a", "(ready for delivery)");

            Assert.IsTrue(73 < mappings.Count);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var d = new ConsoleDedsService();

            Assert.IsTrue(true);
        }

        [TestMethod]
        [TestCategory("ExternalDependency")]
        public void DownLoadAndExtractLars()
        {
            var meta = new MetaDataCreation(null, null, null);
            var filename = "lars.zip";
            var storageFolder = @"C:\Temp\Test\Lars";
            var zipFile = $"{storageFolder}\\{filename}";
            var LarsZipFileUrl = "https://hub.imservices.org.uk/Learning%20Aims/Downloads/Documents/20160209_LARS_1516_CSV.Zip";

            // Clean dir
            DirectoryHelper.DeleteRecursive(storageFolder);

            // Download and unzip
            meta.DownloadFile(LarsZipFileUrl, storageFolder, filename);
            meta.UnPackZipFile(zipFile, $"{storageFolder}\\extracted", "standard.csv");

            // Assert
            var dir = Directory.Exists(@"C:\Temp\Test\Lars");
            var file = File.Exists($@"C:\Temp\Test\Lars\extracted\standard.csv");

            Assert.IsTrue(dir);
            Assert.IsTrue(file);
        }

        [TestMethod]
        public void ExtractFiles()
        {
            Directory.Delete(@"C:\Temp\Test");
            var meta = new MetaDataCreation(null, null, null);
            //var zipFilePath = @"C:\Temp\Test\TestDataLard.zip";
            //var extractedPath = @"C:\Temp\Test\extracted";

            var zipFilePath = @"C:\Temp\Test\Lars\lars.zip";
            var extractedPath = @"C:\Temp\Test\Lars\extracted";

            meta.UnPackZipFile(zipFilePath, extractedPath, "standard.csv");
        }
    }
}
