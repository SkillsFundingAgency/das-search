using System.Collections.Generic;
using System.Linq;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Shared.Components.ViewComponents.ApprenticeshipDetails;
using SFA.DAS.Apprenticeships.Api.Types.AssessmentOrgs;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class AssessmentOrganisationViewModelMapper : IAssessmentOrganisationViewModelMapper
    {
        public AssessmentOrganisationViewModel Map(Organisation item)
        {
            if (item == null) return null;

            var assessmentOrganisation = new AssessmentOrganisationViewModel();

            assessmentOrganisation.Name = item.Name;
            assessmentOrganisation.Website = item.Website;
            assessmentOrganisation.Email = item.Email;
            assessmentOrganisation.Phone = item.Phone;

            return assessmentOrganisation;
        }
    }
}
