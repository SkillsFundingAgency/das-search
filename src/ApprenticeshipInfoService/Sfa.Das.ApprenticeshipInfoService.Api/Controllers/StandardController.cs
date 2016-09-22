namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Net;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;

    using Sfa.Das.ApprenticeshipInfoService.Core.Models;
    using Sfa.Das.ApprenticeshipInfoService.Core.Services;
    using Swashbuckle.Swagger.Annotations;

    public class StandardController : ApiController
    {
        private readonly IGetStandards _getStandards;

        public StandardController(IGetStandards getStandards)
        {
            _getStandards = getStandards;
        }

        // GET api/values/5
        [SwaggerOperation("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public Standard Get(int id)
        {
            var standard = _getStandards.GetStandardById(id);

            if (standard == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return standard;
        }
    }
}
