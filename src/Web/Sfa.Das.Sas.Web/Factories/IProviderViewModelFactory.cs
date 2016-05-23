using Sfa.Das.Sas.Web.Models;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Factories
{
    public interface IProviderViewModelFactory
    {
        ProviderCourseViewModel GenerateDetailsViewModel(ProviderLocationSearchCriteria criteria);
    }
}