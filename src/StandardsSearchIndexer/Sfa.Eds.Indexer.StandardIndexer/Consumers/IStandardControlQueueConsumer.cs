using System.Threading.Tasks;

namespace Sfa.Eds.Das.StandardIndexer.Consumers
{
    public interface IStandardControlQueueConsumer
    {
        Task CheckMessage();
    }
}