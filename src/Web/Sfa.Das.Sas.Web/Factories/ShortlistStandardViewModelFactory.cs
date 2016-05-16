using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public class ShortlistStandardViewModelFactory : IShortlistStandardViewModelFactory
    {
        public ShortlistStandardViewModel GetShortlistStandardViewModel(int standardId, string title, int level)
        {
            return new ShortlistStandardViewModel()
            {
                Id = standardId,
                Title = title,
                Level = level
            };
        }
    }
}