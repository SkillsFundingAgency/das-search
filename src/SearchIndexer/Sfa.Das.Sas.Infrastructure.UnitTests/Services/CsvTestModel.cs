using LINQtoCSV;

namespace Sfa.Das.Sas.Indexer.Infrastructure.UnitTests.Services
{
    public class CsvTestModel
    {
        [CsvColumn(Name = "UKPRN", FieldIndex = 1)]
        public string UkPrn { get; set; }
    }
}