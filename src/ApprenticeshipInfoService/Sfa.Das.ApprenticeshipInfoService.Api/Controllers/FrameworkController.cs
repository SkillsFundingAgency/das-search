using Sfa.Das.ApprenticeshipInfoService.Core.Models;
using Sfa.Das.ApprenticeshipInfoService.Core.Services;

namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Http;
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
            return _getFrameworks.GetFrameworkById(id);
        }
    }
}
