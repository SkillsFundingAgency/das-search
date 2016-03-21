namespace Sfa.Eds.Das.Indexer.Core.Models.Provider
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public ContactInformation Contact { get; set; }
    }
}