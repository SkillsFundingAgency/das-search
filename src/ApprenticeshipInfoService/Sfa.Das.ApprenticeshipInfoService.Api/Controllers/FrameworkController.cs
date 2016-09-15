using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Sfa.Das.ApprenticeshipInfoService.Api.Helpers;
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
        private readonly IControllerHelper _controllerHelper;

        public FrameworkController(IGetFrameworks getFrameworks,
            IControllerHelper controllerHelper)
        {
            _getFrameworks = getFrameworks;
            _controllerHelper = controllerHelper;
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
            return Url.Link("DefaultApi", new { controller = "Framework", id = id });
        }
    }
}
