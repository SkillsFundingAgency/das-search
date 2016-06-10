using System.Collections.Generic;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    using Sfa.Das.Sas.Web.Factories.Interfaces;

    public class ShortlistFrameworkViewModelFactory : IShortlistFrameworkViewModelFactory
    {
        public ShortlistFrameworkViewModel GetShortlistFrameworkViewModel(int frameworkId, string title, int level)
        {
            return new ShortlistFrameworkViewModel()
            {
                Id = frameworkId,
                Title = title,
                Level = level,
                Providers = new List<ShortlistProviderViewModel>()
            };
        }
    }
}