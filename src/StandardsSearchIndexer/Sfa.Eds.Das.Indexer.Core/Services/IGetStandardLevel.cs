namespace Sfa.Eds.Das.Indexer.Core.Services
{
    using System.Data;

    public interface IGetStandardLevel
    {
        int GetFrameworks(int frameworkId);
        int GetNotationLevel(int standardId);
    }
}