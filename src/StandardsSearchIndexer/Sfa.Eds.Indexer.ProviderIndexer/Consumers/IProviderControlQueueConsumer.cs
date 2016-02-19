using System.Threading.Tasks;

namespace Sfa.Eds.Das.ProviderIndexer.Consumers
{
    public interface IProviderControlQueueConsumer
    {
        Task CheckMessage();
    }
}