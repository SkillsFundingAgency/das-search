using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories.Interfaces
{
    public interface IShortlistViewModelFactory
    {
        IShortlistApprenticeshipViewModel GetShortlistViewModel(Standard standard);
        IShortlistApprenticeshipViewModel GetShortlistViewModel(Framework framework);
    }
}
