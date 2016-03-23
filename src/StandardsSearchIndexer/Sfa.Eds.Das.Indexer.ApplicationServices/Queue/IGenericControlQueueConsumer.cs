namespace Sfa.Eds.Das.Indexer.ApplicationServices.Queue
{
    using System.Threading.Tasks;
    using Services;
    using Sfa.Eds.Das.Indexer.Core.Models;

    public interface IGenericControlQueueConsumer
    {
        Task CheckMessage<T>()
            where T : IMaintainSearchIndexes;
    }
}