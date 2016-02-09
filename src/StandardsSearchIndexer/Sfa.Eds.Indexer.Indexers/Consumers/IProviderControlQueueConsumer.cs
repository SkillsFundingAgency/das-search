using System.Threading.Tasks;

namespace Sfa.Eds.Indexer.Indexers.Consumers
{
    public interface IProviderControlQueueConsumer
    {
        Task CheckMessage();
    }
}