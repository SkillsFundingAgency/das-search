namespace Sfa.Das.Sas.Web.Factories.Interfaces
{
    using Sfa.Das.Sas.Web.ViewModels;

    public interface IShortlistFrameworkViewModelFactory
    {
        ShortlistFrameworkViewModel GetShortlistFrameworkViewModel(int frameworkId, string title, int level);
    }
}
