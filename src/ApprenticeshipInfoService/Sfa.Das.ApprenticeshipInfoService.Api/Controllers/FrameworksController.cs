namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Http;
    using Sfa.Das.ApprenticeshipInfoService.Api.Helpers;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;
    using Sfa.Das.ApprenticeshipInfoService.Core.Services;
    using Swashbuckle.Swagger.Annotations;

    public class FrameworksController : ApiController
    {
        private readonly IGetFrameworks _getFrameworks;

        public FrameworksController(IGetFrameworks getFrameworks)
        {
            _getFrameworks = getFrameworks;
        }

        // GET api/values
        [SwaggerOperation("GetAll")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IEnumerable<FrameworkSummary> Get()
        {
            return _getFrameworks.GetAllFrameworks();
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public Framework GetFramework(int id)
        {
            var response = _getFrameworks.GetFrameworkById(id);
            if (response != null)
            {
                return response;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // HEAD api/values/5
        public void Head(int id)
        {
            if (_getFrameworks.GetFrameworkById(id) != null)
            {
                return;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        private string Resolve(int id)
        {
            return Url.Link("DefaultApi", new { controller = "Frameworks", id = id });
        }
    }
}
