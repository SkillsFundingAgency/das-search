namespace Sfa.Eds.Das.Indexer.Common.UnitTests.Extensions
{
    using LINQtoCSV;

    public class CsvTestModel
    {
        [CsvColumn(Name = "UKPRN", FieldIndex = 1)]
        public string UkPrn { get; set; }
    }
}