using System.Threading.Tasks;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Queue
{
    public interface IGenericControlQueueConsumer
    {
        Task CheckMessage<T>()
            where T : IMaintainSearchIndexes;
    }
}