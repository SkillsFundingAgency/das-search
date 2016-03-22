namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System.Data;

    public interface IGetStandardLevel
    {
        int GetNotationLevel(int standardId);
    }
}