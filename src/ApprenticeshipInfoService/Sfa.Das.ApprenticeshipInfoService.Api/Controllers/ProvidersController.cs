using System;
using Sfa.Das.ApprenticeshipInfoService.Api.Helpers;
using IControllerHelper = Sfa.Das.ApprenticeshipInfoService.Core.Helpers.IControllerHelper;

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
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;

        public ProvidersController(
            IGetProviders getProviders,
            IControllerHelper controllerHelper,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository)
        {
            _getProviders = getProviders;
            _controllerHelper = controllerHelper;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
        }

        // GET standards/5/providers?lat=<latitude>&long=<longitude>&page=#
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("standards/{id}/providers")]
        public List<StandardProviderSearchResultsItem> GetByStandardIdAndLocation(int id, double? lat = null, double? lon = null, int page = 1)
        {
            // TODO 404 if standard doesn't exists
            var actualPage = _controllerHelper.GetActualPage(page);

            if (lat.HasValue && lon.HasValue)
            {
                return _getProviders.GetByStandardIdAndLocation(id, lat.Value, lon.Value, actualPage);
            }

            throw HttpResponseFactory.RaiseException(HttpStatusCode.BadRequest, "A valid Latitude and Longitude is required");
        }

        // GET frameworks/5/providers?lat=<latitude>&long=<longitude>&page=#
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("frameworks/{id}/providers")]
        public List<FrameworkProviderSearchResultsItem> GetByFrameworkIdAndLocation(int id, double? lat = null, double? lon = null, int page = 1)
        {
            // TODO 404 if framework doesn't exists
            var actualPage = _controllerHelper.GetActualPage(page);

            if (lat.HasValue && lon.HasValue)
            {
                return _getProviders.GetByFrameworkIdAndLocation(id, lat.Value, lon.Value, actualPage);
            }

            throw HttpResponseFactory.RaiseException(HttpStatusCode.BadRequest, "A valid Latitude and Longitude is required");
        }

        // GET standards/<standardId>/providers?ukprn=<ukprn>&location=<locationId>
        [SwaggerOperation("GetStandardProviderDetails")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("standards/{standardCode}/providers")]
        public ApprenticeshipDetails GetStandardProviderDetails(string standardCode, int? ukprn = null, int? location = null)
        {
            if (ukprn.HasValue && location.HasValue)
            {
                var model = _apprenticeshipProviderRepository.GetCourseByStandardCode(
                    ukprn.Value,
                    location.Value,
                    standardCode);

                if (model != null)
                {
                    return model;
                }

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            throw HttpResponseFactory.RaiseException(HttpStatusCode.BadRequest, "A valid Ukprn and Location is required");
        }

        // GET frameworks/<frameworkId>/providers?ukprn=<ukprn>&location=<locationId>
        [SwaggerOperation("GetStandardProviderDetails")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("frameworks/{frameworkId}/providers")]
        public ApprenticeshipDetails GetFrameworkProviderDetails(string frameworkId, int? ukprn = null, int? location = null)
        {
            if (ukprn.HasValue && location.HasValue)
            {
                var model = _apprenticeshipProviderRepository.GetCourseByFrameworkId(
                    ukprn.Value,
                    location.Value,
                    frameworkId);

                if (model != null)
                {
                    return model;
                }

                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            throw HttpResponseFactory.RaiseException(HttpStatusCode.BadRequest, "A valid Ukprn and Location is required");
        }
    }
}