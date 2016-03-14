namespace Sfa.Infrastructure.Elasticsearch
{
    public interface IGenerateIndexDefinitions<T>
    {
        string Generate();
    }
}
