namespace Sfa.Infrastructure.UnitTests.Services
{
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    using Sfa.Infrastructure.Services;

    [TestFixture]
    public class CsvServiceTests
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
            var csv = string.Empty;
            var sut = new CsvService();

            var models = sut.CsvTo<CsvTestModel>(csv);

            Assert.AreEqual(0, models.Count);
        }
    }
}