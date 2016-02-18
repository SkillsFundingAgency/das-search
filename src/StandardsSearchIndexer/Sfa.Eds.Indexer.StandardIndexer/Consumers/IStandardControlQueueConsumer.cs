using System.Threading.Tasks;

namespace Sfa.Eds.StandardIndexer.Consumers
{
    public interface IStandardControlQueueConsumer
    {
        Task CheckMessage();
    }
}