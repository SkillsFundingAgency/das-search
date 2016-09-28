namespace Sfa.Das.ApprenticeshipInfoService.Core.Helpers
{
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;

    public interface IControllerHelper
    {
        int GetActualPage(int page);

        DetailProviderResponse CreateDetailProviderResponse(ApprenticeshipDetails model, IApprenticeshipProduct apprenticeshipProduct, ApprenticeshipTrainingType apprenticeshipProductType);
    }
}
