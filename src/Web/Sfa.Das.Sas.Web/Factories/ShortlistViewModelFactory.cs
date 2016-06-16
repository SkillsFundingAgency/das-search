using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.Factories.Interfaces;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public class ShortlistViewModelFactory : IShortlistViewModelFactory
    {
        public IShortlistApprenticeshipViewModel GetShortlistViewModel(Standard standard)
        {
            if (standard == null)
            {
                return null;
            }

            return new ShortlistStandardViewModel()
            {
                Id = standard.StandardId,
                Title = standard.Title,
                Level = standard.NotionalEndLevel,
                Providers = new List<ShortlistProviderViewModel>()
            };
        }

        public IShortlistApprenticeshipViewModel GetShortlistViewModel(Framework framework)
        {
            if (framework == null)
            {
                return null;
            }

            return new ShortlistFrameworkViewModel()
            {
                Id = framework.FrameworkId,
                Title = framework.Title,
                Level = framework.Level,
                Providers = new List<ShortlistProviderViewModel>()
            };
        }
    }
}