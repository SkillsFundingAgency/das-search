using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public interface IShortlistFrameworkViewModelFactory
    {
        ShortlistFrameworkViewModel GetShortlistFrameworkViewModel(int frameworkId, string title, int level);
    }
}
