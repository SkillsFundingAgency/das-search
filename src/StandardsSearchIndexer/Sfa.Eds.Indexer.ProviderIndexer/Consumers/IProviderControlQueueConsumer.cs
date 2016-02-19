using System.Threading.Tasks;

namespace Sfa.Eds.ProviderIndexer.Consumers
{
    public interface IProviderControlQueueConsumer
    {
        Task CheckMessage();
    }
}