namespace Sfa.Eds.Das.Indexer.Common.UnitTests.Extensions
{
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using Sfa.Eds.Das.Indexer.Common.Extensions;
    using Sfa.Infrastructure.Services;

    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void ShouldFindTheValueOfACellInACSV()
        {
            var csv = "UKPRN\n10001309\n10031241";
            var sut = new CsvService();

            var models = sut.CsvTo<CsvTestModel>(csv);

            Assert.AreEqual("10001309", models.First().UkPrn);
        }

        [Test]
        public void ShouldReturnElementCountFromCsv()
        {
            var csv = "UKPRN\n10001309\n10031241";
            var sut = new CsvService();

            var models = sut.CsvTo<CsvTestModel>(csv);

            Assert.AreEqual(2, models.Count);
        }

        [Test]
        public void ShouldReturnEmptyEnumerableIfCsvDoesntContainAnything()
        {
            var csv = "";
            var sut = new CsvService();

            var models = sut.CsvTo<CsvTestModel>(csv);

            Assert.AreEqual(0, models.Count);
        }

        [Test]
        public void ShouldConvertAStringToAStream()
        {
            var input = "test";
            var stream = input.GenerateStreamFromString();
            var result = new StreamReader(stream).ReadToEnd();

            Assert.AreEqual(input, result);
        }
    }
}