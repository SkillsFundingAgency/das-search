namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Net;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;
    using Sfa.Das.ApprenticeshipInfoService.Core.Services;
    using Swashbuckle.Swagger.Annotations;

    public class ProvidersController
    {
        private readonly IGetProviders _getProviders;

        public ProvidersController(IGetProviders getProviders)
        {
            _getProviders = getProviders;
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public StandardProviderSearchResultsItem Get(int standardId, int orderId)
        {
            return new StandardProviderSearchResultsItem();
        }
    }
}