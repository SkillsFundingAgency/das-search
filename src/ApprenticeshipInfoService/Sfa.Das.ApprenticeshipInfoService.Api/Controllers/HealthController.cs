namespace Sfa.Das.ApprenticeshipInfoService.Api.Controllers
{
    using System.Web.Http;
    using Health;
    using Health.Elasticsearch;

    public class HealthController : ApiController
    {
        private readonly IElasticsearchService _elasticsearchService;

        public HealthController(IElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        // GET: Health
        public HealthModel Get()
        {
            return _elasticsearchService.GetHealth();
        }
    }
}