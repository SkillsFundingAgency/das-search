using SFA.DAS.Apprenticeships.Api.Types;

namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    using Sfa.Das.ApprenticeshipInfoService.Api.Attributes;
    using Sfa.Das.ApprenticeshipInfoService.Api.Helpers;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models.Responses;
    using Sfa.Das.ApprenticeshipInfoService.Core.Services;
    using Swashbuckle.Swagger.Annotations;
    using IControllerHelper = Sfa.Das.ApprenticeshipInfoService.Core.Helpers.IControllerHelper;

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

        // GET /providers
        [SwaggerOperation("GetAll")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("providers")]
        [ExceptionHandling]
        public IEnumerable<Provider> Get()
        {
            var response = _getProviders.GetAllProviders();

            return response;
        }

        // GET /providers
        [SwaggerOperation("GetByUkprn")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("providers/{ukprn}")]
        [ExceptionHandling]
        public IEnumerable<Provider> Get(int ukprn)
        {
            var response = _getProviders.GetProvidersByUkprn(ukprn);

            if (!response.Any())
            {
                throw HttpResponseFactory.RaiseException(HttpStatusCode.NotFound, string.Format("No provider with Ukprn {0} found", ukprn));
            }

            return response;
        }

        // GET standards/5/providers?lat=<latitude>&long=<longitude>&page=#
        [SwaggerOperation("GetByStandardIdAndLocation")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(List<StandardProviderSearchResultsItem>))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("standards/{id}/providers")]
        public List<StandardProviderSearchResultsItemResponse> GetByStandardIdAndLocation(int id, double? lat = null, double? lon = null, int page = 1)
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
        [SwaggerOperation("GetByFrameworkIdAndLocation")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(List<FrameworkProviderSearchResultsItem>))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("frameworks/{id}/providers")]
        [ExceptionHandling]
        public List<FrameworkProviderSearchResultsItemResponse> GetByFrameworkIdAndLocation(int id, double? lat = null, double? lon = null, int page = 1)
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
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(ApprenticeshipDetails))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("standards/{standardCode}/providers")]
        [ExceptionHandling]
        public ApprenticeshipDetails GetStandardProviderDetails(string standardCode, int ukprn, int location)
        {
            var model = _apprenticeshipProviderRepository.GetCourseByStandardCode(
                ukprn,
                location,
                standardCode);

            if (model != null)
            {
                return model;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // GET frameworks/<frameworkId>/providers?ukprn=<ukprn>&location=<locationId>
        [SwaggerOperation("GetFrameworkProviderDetails")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(ApprenticeshipDetails))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("frameworks/{frameworkId}/providers")]
        [ExceptionHandling]
        public ApprenticeshipDetails GetFrameworkProviderDetails(string frameworkId, int ukprn, int location)
        {
            var model = _apprenticeshipProviderRepository.GetCourseByFrameworkId(
                ukprn,
                location,
                frameworkId);

            if (model != null)
            {
                return model;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}