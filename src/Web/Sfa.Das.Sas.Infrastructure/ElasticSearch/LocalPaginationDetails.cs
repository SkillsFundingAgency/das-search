namespace Sfa.Das.Sas.Infrastructure.Elasticsearch
{
    public struct LocalPaginationDetails
    {
        public int Skip { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
    }
}