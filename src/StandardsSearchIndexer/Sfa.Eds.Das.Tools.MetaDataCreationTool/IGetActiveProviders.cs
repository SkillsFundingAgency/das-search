namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGetActiveProviders
    {
        Task<IEnumerable<string>> GetActiveUkPrns();
    }
}