namespace Sfa.Das.Sas.Web.Factories.Interfaces
{
    using Sfa.Das.Sas.Web.ViewModels;

    public interface IShortlistStandardViewModelFactory
    {
        ShortlistStandardViewModel GetShortlistStandardViewModel(int standardId, string title, int level);
    }
}
