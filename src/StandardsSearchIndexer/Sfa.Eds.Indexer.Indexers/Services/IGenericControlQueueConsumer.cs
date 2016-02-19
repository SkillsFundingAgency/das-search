using System.Threading.Tasks;

namespace Sfa.Eds.Das.Indexer.Common.Services
{
    public interface IGenericControlQueueConsumer
    {
        Task CheckMessage<T>() where T : IIndexerService;
    }
}