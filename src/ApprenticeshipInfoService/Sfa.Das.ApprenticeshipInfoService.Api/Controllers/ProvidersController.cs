namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Http;
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
        [Route("standards/{id}/providers")]
        public List<StandardProviderSearchResultsItem> GetByStandardIdAndLocation(int id, double? lat, double? lon, int? page)
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

            if (lat != null && lon != null)
            {
                return _getProviders.GetByStandardIdAndLocation(id, (double)lat, (double)lon, actualPage);
            }

            return new List<StandardProviderSearchResultsItem>();
        }

        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("frameworks/{id}/providers")]
        public List<FrameworkProviderSearchResultsItem> GetByFrameworkIdAndLocation(int id, double? lat, double? lon, int? page)
        {
            var actualPage = 1;
            if (page != null)
            {
                actualPage = (int) page;
            }

            if (actualPage < 1)
            {
                actualPage = 1;
            }

            if (lat != null && lon != null)
            {
                return _getProviders.GetByFrameworkIdAndLocation(id, (double)lat, (double)lon, actualPage);
            }

            return new List<FrameworkProviderSearchResultsItem>();
        }
    }
}