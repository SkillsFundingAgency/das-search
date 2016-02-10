using System.Threading.Tasks;

namespace Sfa.Eds.Indexer.ProviderIndexer.Consumers
{
    public interface IProviderControlQueueConsumer
    {
        Task CheckMessage();
    }
}