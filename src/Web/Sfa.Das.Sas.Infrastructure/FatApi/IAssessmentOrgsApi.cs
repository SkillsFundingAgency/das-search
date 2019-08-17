using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Sfa.Das.FatApi.Client.Model;

namespace Sfa.Das.FatApi.Client.Api
{
    public interface IAssessmentOrgsApi
    {
        /// <summary>
        /// Get a single organisation details
        /// GET /assessmentorgs/{organisationId}
        /// </summary>
        /// <param name="organisationId">a string for the organisation id</param>
        /// <returns>a organisation details based on id</returns>
        [Get("/assessmentorgs/{organisationId}")]
        Task<Organisation> GetAsync(string organisationId);

        /// <summary>
        /// Get a collection of organisations
        /// GET /assessment-organisations/standards/{standardId}
        /// </summary>
        /// <param name="standardId">an integer for the standard id</param>
        /// <returns>a collection of organisation</returns>
        [Get("/assessment-organisations/standards/{standardId}")]
        Task<IEnumerable<Organisation>> ByStandardAsync(int standardId);

        /// <summary>
        /// Get a collection of organisations
        /// GET /frameworks
        /// </summary>
        /// <returns>a collection of organisation summaries</returns>
        [Get("/assessment-organisations")]
        Task<IEnumerable<OrganisationSummary>> FindAllAsync();

        /// <summary>
        /// Check if a assessment organisation exists
        /// HEAD /assessmentorgs/{organisationId}
        /// </summary>
        /// <param name="organisationId">a string for the organisation id</param>
        /// <returns>bool</returns>
        [Get("/assessmentorgs/{organisationId}")]
        Task<bool> ExistsAsync(string organisationId);

        /// <summary>
        /// Get a collection of standards
        /// GET /assessment-organisations/{organisationId}/standards
        /// </summary>
        /// /// <param name="organisationId">a string for the organisation id</param>
        /// <returns>a collection of standards</returns>
        [Get("/assessment-organisations/{organisationId}/standards")]        
        Task<IEnumerable<StandardOrganisationSummary>> FindAllStandardsByOrganisationIdAsync(string organisationId);
    }
}