namespace Sfa.Eds.Das.ProviderIndexer.Models
{
    using LINQtoCSV;

    public class FcsProviderRecord
    {
        [CsvColumn(Name = "UKPRN", FieldIndex = 1)]
        public string UkPrn { get; set; }
    }
}