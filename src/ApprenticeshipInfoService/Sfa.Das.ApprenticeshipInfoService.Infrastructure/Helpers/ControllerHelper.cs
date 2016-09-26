using Sfa.Das.ApprenticeshipInfoService.Core.Helpers;
using Sfa.Das.ApprenticeshipInfoService.Core.Models;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Helpers
{
    public class ControllerHelper : IControllerHelper
    {
        public int GetActualPage(int? page)
        {
            var actualPage = 1;
            if (page != null)
            {
                actualPage = (int)page;
            }

            if (actualPage < 1)
            {
                actualPage = 1;
            }

            return actualPage;
        }

        public DetailProviderResponse CreateDetailProviderResponse(ApprenticeshipDetails model, IApprenticeshipProduct apprenticeshipProduct, ApprenticeshipTrainingType apprenticeshipProductType)
        {
            if (model == null || apprenticeshipProduct == null)
            {
                return new DetailProviderResponse
                {
                    StatusCode = DetailProviderResponse.ResponseCodes.ApprenticeshipProviderNotFound
                };
            }

            var response = new DetailProviderResponse
            {
                StatusCode = DetailProviderResponse.ResponseCodes.Success,
                ApprenticeshipDetails = model,
                ApprenticeshipType = apprenticeshipProductType,
                ApprenticeshipName = apprenticeshipProduct.Title,
                ApprenticeshipLevel = apprenticeshipProduct.Level.ToString()
            };

            return response;
        }
    }
}
