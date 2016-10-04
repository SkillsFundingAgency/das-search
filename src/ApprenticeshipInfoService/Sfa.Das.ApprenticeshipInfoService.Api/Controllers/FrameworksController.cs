using System.Linq;

namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Http;
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

        // GET /frameworks
        [SwaggerOperation("GetAll")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [Route("frameworks")]
        public IEnumerable<FrameworkSummary> Get()
        {
            var response = _getFrameworks.GetAllFrameworks().ToList();

            foreach (var item in response)
            {
                item.Uri = Resolve(item.Id);
            }

            return response;
        }

        // GET /frameworks/40338

        /// <summary>
        /// Get a framework by composite id
        /// </summary>
        /// <param name="id">{FrameworkId}{ProgType}{PathwayId} ie: 40338</param>
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("frameworks/{id}")]
        public Framework Get(int id)
        {
            if (_getFrameworks.GetFrameworkById(id) != null)
            {
                var response = _getFrameworks.GetFrameworkById(id);
                response.Uri = Resolve(response.FrameworkId);
                return response;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // HEAD /frameworks/5

        /// <summary>
        /// Get a framework by composite id
        /// </summary>
        /// <param name="id">{FrameworkId}{ProgType}{PathwayId} ie: 40338</param>
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("frameworks/{id}")]
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
            return Url.Link("DefaultApi", new { controller = "frameworks", id = id });
        }
    }
}
