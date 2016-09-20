using System.Collections.Generic;
using System.Web.Http;

namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Net;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;
    using Sfa.Das.ApprenticeshipInfoService.Core.Services;
    using Swashbuckle.Swagger.Annotations;

    public class ProvidersController : ApiController
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
        [Route("api/standard/{id}/providers")]
        public List<StandardProviderSearchResultsItem> GetByStandardIdAndLocation(int id, double? lat, double? lon)
        {
            if (lat != null && lon != null)
            {
                return _getProviders.GetByStandardIdAndLocation(id, (double)lat, (double)lon);
            }

            return new List<StandardProviderSearchResultsItem>();
        }
    }
}