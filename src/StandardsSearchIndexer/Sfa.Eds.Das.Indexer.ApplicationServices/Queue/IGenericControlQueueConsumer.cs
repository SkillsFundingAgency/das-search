namespace Sfa.Eds.Das.Indexer.ApplicationServices.Queue
{
    using System.Threading.Tasks;

    using Sfa.Eds.Das.Indexer.Common.Models;

    public interface IGenericControlQueueConsumer
    {
        Task CheckMessage<T>() where T : IIndexEntry;
    }
}