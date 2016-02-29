using System.Threading.Tasks;

namespace Sfa.Eds.Das.Indexer.Common.Services
{
    using Sfa.Eds.Das.Indexer.Common.Models;

    public interface IGenericControlQueueConsumer
    {
        Task CheckMessage<T>()
            where T : IIndexEntry;
    }
}