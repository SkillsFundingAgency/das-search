namespace Sfa.Infrastructure.UnitTests.Services
{
    using LINQtoCSV;

    public class CsvTestModel
    {
        [CsvColumn(Name = "UKPRN", FieldIndex = 1)]
        public string UkPrn { get; set; }
    }
}