using System;
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
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;

        public ProvidersController(
            IGetProviders getProviders,
            IControllerHelper controllerHelper,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository,
            IGetStandards getStandards,
            IGetFrameworks getFrameworks)
        {
            _getProviders = getProviders;
            _controllerHelper = controllerHelper;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
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

        // GET standards/<standardId>/providers?ukprn=<ukprn>&location=<locationId>
        [SwaggerOperation("GetStandardProviderDetails")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("standards/{standardCode}/providers")]
        public ApprenticeshipDetails GetStandardProviderDetails(string standardCode, int? ukprn, int? location)
        {
            if (ukprn != null && location != null)
            {
                var model = _apprenticeshipProviderRepository.GetCourseByStandardCode(
                    (int)ukprn,
                    (int)location,
                    standardCode);

                return model;
            }

            return new ApprenticeshipDetails();
        }

        // GET frameworks/<frameworkId>/providers?ukprn=<ukprn>&location=<locationId>
        [SwaggerOperation("GetStandardProviderDetails")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("frameworks/{frameworkId}/providers")]
        public ApprenticeshipDetails GetFrameworkProviderDetails(string frameworkId, int? ukprn, int? location)
        {
            if (ukprn != null && location != null)
            {
                var model = _apprenticeshipProviderRepository.GetCourseByFrameworkId(
                    (int)ukprn,
                    (int)location,
                    frameworkId);

                return model;
            }

            return new ApprenticeshipDetails();
        }
    }
}