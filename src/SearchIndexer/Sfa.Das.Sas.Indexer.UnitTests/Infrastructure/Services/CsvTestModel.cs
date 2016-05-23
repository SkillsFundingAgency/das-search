using LINQtoCSV;

namespace Sfa.Das.Sas.Indexer.UnitTests.Infrastructure.Services
{
    public class CsvTestModel
    {
        [CsvColumn(Name = "UKPRN", FieldIndex = 1)]
        public string UkPrn { get; set; }
    }
}