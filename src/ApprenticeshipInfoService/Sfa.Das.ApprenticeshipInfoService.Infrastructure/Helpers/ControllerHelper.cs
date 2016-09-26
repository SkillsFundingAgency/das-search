using Sfa.Das.ApprenticeshipInfoService.Core.Helpers;

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
    }
}
