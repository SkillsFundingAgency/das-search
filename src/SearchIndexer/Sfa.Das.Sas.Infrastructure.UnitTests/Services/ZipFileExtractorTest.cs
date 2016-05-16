namespace Sfa.Das.Sas.Indexer.Infrastructure.UnitTests.Services
{
    using System.IO;
    using System.IO.Compression;

    using FluentAssertions;

    using NUnit.Framework;

    using Sfa.Das.Sas.Indexer.Infrastructure.Services;

    [TestFixture]
    public class ZipFileExtractorTest
    {
        [TestCase("CSV/frameworks.csv", "frameworks.csv", "Bar!", "Bar!")]
        [TestCase("CSVNOT/frameworks.csv", "frameworks.csv", "Bar!", null)]
        public void ShouldUnzipStream(string filePath, string fileName, string content, string result)
        {
            ZipFileExtractor zipFileExtractor = new ZipFileExtractor();

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var frameworkFile = archive.CreateEntry(filePath);

                    using (var entryStream = frameworkFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(content);
                    }
                }

                var frmeworksString = zipFileExtractor.ExtractFileFromStream(memoryStream, fileName);
                frmeworksString.Should().Equals(result);
            }
        }

        [TestCase("CSV/frameworks.csv", "standards.csv", "Bar!")]
        [TestCase("CSVNOT/frameworks.csv", "frameworks.csv", "Bar!")]
        public void ShouldFailToUnzipStream(string filePath, string fileName, string content)
        {
            ZipFileExtractor zipFileExtractor = new ZipFileExtractor();

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var frameworkFile = archive.CreateEntry(filePath);

                    using (var entryStream = frameworkFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(content);
                    }
                }

                var frmeworksString = zipFileExtractor.ExtractFileFromStream(memoryStream, fileName);
                frmeworksString.Should().BeNull();
            }
        }
    }
}
