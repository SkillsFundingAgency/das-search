namespace Sfa.Eds.Das.Indexer.ApplicationServices.Provider
{
    using LINQtoCSV;

    public class ActiveProviderCsvRecord
    {
        [CsvColumn(Name = "UKPRN", FieldIndex = 1)]
        public string UkPrn { get; set; }
    }
}