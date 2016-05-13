using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public interface IShortlistStandardViewModelFactory
    {
        ShortlistStandardViewModel GetShortlistStandardViewModel(int standardId, string title, int level);
    }
}
