using Sfa.Das.ApprenticeshipInfoService.Core.Helpers;

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
        private readonly IControllerHelper _controllerHelper;

        public ProvidersController(
            IGetProviders getProviders,
            IControllerHelper controllerHelper)
        {
            _getProviders = getProviders;
            _controllerHelper = controllerHelper;
        }

        // GET standards/5/providers?lat=<latitude>&long=<longitude>&page=#
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("standards/{id}/providers")]
        public List<StandardProviderSearchResultsItem> GetByStandardIdAndLocation(int id, double? lat, double? lon, int? page)
        {
            var actualPage = _controllerHelper.GetActualPage(page);

            if (lat != null && lon != null)
            {
                return _getProviders.GetByStandardIdAndLocation(id, (double)lat, (double)lon, actualPage);
            }

            return new List<StandardProviderSearchResultsItem>();
        }

        // GET frameworks/5/providers?lat=<latitude>&long=<longitude>&page=#
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("frameworks/{id}/providers")]
        public List<FrameworkProviderSearchResultsItem> GetByFrameworkIdAndLocation(int id, double? lat, double? lon, int? page)
        {
            var actualPage = _controllerHelper.GetActualPage(page);

            if (lat != null && lon != null)
            {
                return _getProviders.GetByFrameworkIdAndLocation(id, (double)lat, (double)lon, actualPage);
            }

            return new List<FrameworkProviderSearchResultsItem>();
        }
    }
}