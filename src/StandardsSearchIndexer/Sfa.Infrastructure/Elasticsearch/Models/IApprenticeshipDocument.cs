namespace Sfa.Infrastructure.Elasticsearch.Models
{
    public interface IApprenticeshipDocument
    {
        string Title { get; set; }
        int Level { get; set; }
    }
}
