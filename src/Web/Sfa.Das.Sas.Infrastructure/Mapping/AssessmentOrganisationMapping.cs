using Sfa.Das.FatApi.Client.Model;
using Sfa.Das.Sas.Core.Domain;

namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    public class AssessmentOrganisationMapping : IAssessmentOrganisationMapping
    {
        public AssessmentOrganisation Map(Organisation document)
        {
            if (document == null) return null;

            var assessmentOrgainisation = new AssessmentOrganisation()
            {
                Name = document.Name,
                Email = document.Email,
                Phone = document.Phone,
                Website = document.Website
            };
            return assessmentOrgainisation;
        }
    }
}
