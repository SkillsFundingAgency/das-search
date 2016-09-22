namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Http;
    using Sfa.Das.ApprenticeshipInfoService.Api.Helpers;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;
    using Sfa.Das.ApprenticeshipInfoService.Core.Services;
    using Swashbuckle.Swagger.Annotations;

    public class StandardsController : ApiController
    {
        private readonly IGetStandards _getStandards;

        public StandardsController(IGetStandards getStandards)
        {
            _getStandards = getStandards;
        }

        // GET /standards
        [SwaggerOperation("GetAll")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IEnumerable<StandardSummary> Get()
        {
            var response = _getStandards.GetAllStandards().ToList();

            foreach (var item in response)
            {
                item.Uri = Resolve(item.Id);
            }

            return response;
        }

        // GET /standards/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public Standard Get(int id)
        {
            var standard = _getStandards.GetStandardById(id);
            if (standard != null)
            {
                standard.Uri = Resolve(standard.StandardId);
                return standard;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // HEAD /standards/5
        public void Head(int id)
        {
            if (_getStandards.GetStandardById(id) != null)
            {
                return;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        private string Resolve(int id)
        {
            return Url.Link("DefaultApi", new { controller = "Standards", id = id });
        }
    }
}
