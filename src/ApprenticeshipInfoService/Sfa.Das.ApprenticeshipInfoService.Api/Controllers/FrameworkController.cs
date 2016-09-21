namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Net;
    using System.Web.Http;
    using Core.Models;
    using Core.Services;
    using Swashbuckle.Swagger.Annotations;

    public class FrameworkController : ApiController
    {
        private readonly IGetFrameworks _getFrameworks;

        public FrameworkController(IGetFrameworks getFrameworks)
        {
            _getFrameworks = getFrameworks;
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public Framework Get(int id)
        {
            var framework = _getFrameworks.GetFrameworkById(id);
            if (framework == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return framework;
        }
    }
}
