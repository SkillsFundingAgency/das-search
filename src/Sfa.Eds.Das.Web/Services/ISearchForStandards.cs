using Sfa.Eds.Das.Web.Models;

namespace Sfa.Eds.Das.Web.Services
{
    public interface ISearchForStandards
    {
        SearchResults Search(string keywords);
    }
}
