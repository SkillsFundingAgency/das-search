using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Das.FatApi.Client.Api;
using Sfa.Das.Sas.Core.Domain;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Infrastructure.Mapping;

namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    public sealed class AssessmentOrganisationApiRepository : IAssessmentOrgsApi, IGetAssessmentOrganisations
    {
        private readonly IAssessmentOrgsApi _apiClient;
        private readonly ILogger<AssessmentOrganisationApiRepository> _logger;
        private readonly IAssessmentOrganisationMapping _assessmentOrganisationMapping;

        public AssessmentOrganisationApiRepository(IAssessmentOrgsApi apiClient, ILogger<AssessmentOrganisationApiRepository> logger, IAssessmentOrganisationMapping assessmentOrganisationMapping)
        {
            _apiClient = apiClient;
            _logger = logger;
            _assessmentOrganisationMapping = assessmentOrganisationMapping;
        }

        public Task<IEnumerable<FatApi.Client.Model.Organisation>> ByStandardAsync(int standardId)
        {
            return _apiClient.ByStandardAsync(standardId);
        }

        public async Task<bool> ExistsAsync(string organisationId)
        {
            return await _apiClient.ExistsAsync(organisationId);
        }

        public Task<IEnumerable<FatApi.Client.Model.OrganisationSummary>> FindAllAsync()
        {
            return _apiClient.FindAllAsync();
        }

        public Task<IEnumerable<FatApi.Client.Model.StandardOrganisationSummary>> FindAllStandardsByOrganisationIdAsync(string organisationId)
        {
            return _apiClient.FindAllStandardsByOrganisationIdAsync(organisationId);
        }

        public Task<FatApi.Client.Model.Organisation> GetAsync(string organisationId)
        {
            return _apiClient.GetAsync(organisationId);
        }

        public async Task<IEnumerable<AssessmentOrganisation>> GetByStandardId(int id)
        {
                var organisations = await this.ByStandardAsync(id);
                return organisations.Select(_assessmentOrganisationMapping.Map);
        }
    }
}
