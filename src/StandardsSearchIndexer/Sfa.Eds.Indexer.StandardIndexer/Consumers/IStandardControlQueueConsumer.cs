using System.Threading.Tasks;

namespace Sfa.Eds.Indexer.StandardIndexer.Consumers
{
    public interface IStandardControlQueueConsumer
    {
        Task CheckMessage();
    }
}