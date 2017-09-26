using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Api.Types.AssessmentOrgs;
using SFA.DAS.AssessmentOrgs.Api.Client;

namespace Sfa.Das.Sas.Infrastructure.Repositories
{
    public sealed class AssessmentOrganisationApiRepository : IAssessmentOrgsApiClient
    {
        private readonly IAssessmentOrgsApiClient _apiClient;

        public AssessmentOrganisationApiRepository(IAssessmentOrgsApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public void Dispose()
        {
            _apiClient.Dispose();
        }

        public Organisation Get(string organisationId)
        {
            var res = _apiClient.Get(organisationId);
            return res;
        }

        public async Task<Organisation> GetAsync(string organisationId)
        {
            var result = await _apiClient.GetAsync(organisationId);
            return result;
        }

        public IEnumerable<Organisation> ByStandard(int standardId)
        {
            var res = _apiClient.ByStandard(standardId);
            return res;
        }

        public async Task<IEnumerable<Organisation>> ByStandardAsync(int standardId)
        {
            var result = await _apiClient.ByStandardAsync(standardId);
            return result;
        }

        public IEnumerable<Organisation> ByStandard(string standardId)
        {
            var res = _apiClient.ByStandard(standardId);
            return res;
        }

        public async Task<IEnumerable<Organisation>> ByStandardAsync(string standardId)
        {
            var result = await _apiClient.ByStandardAsync(standardId);
            return result;
        }

        public IEnumerable<OrganisationSummary> FindAll()
        {
            var res = _apiClient.FindAll();
            return res;
        }

        public async Task<IEnumerable<OrganisationSummary>> FindAllAsync()
        {
            var result = await _apiClient.FindAllAsync();
            return result;
        }

        public bool Exists(string organisationId)
        {
            var res = _apiClient.Exists(organisationId);
            return res;
        }

        public async Task<bool> ExistsAsync(string organisationId)
        {
            var result = await _apiClient.ExistsAsync(organisationId);
            return result;
        }
    }
}
