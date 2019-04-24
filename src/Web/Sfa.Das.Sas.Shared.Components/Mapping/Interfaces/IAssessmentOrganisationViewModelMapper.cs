using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;
using Sfa.Das.Sas.Shared.Components.ViewComponents.ApprenticeshipDetails;
using SFA.DAS.Apprenticeships.Api.Types.AssessmentOrgs;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public interface IAssessmentOrganisationViewModelMapper
    {
        AssessmentOrganisationViewModel Map(Organisation assessmentOrganisation);

    }
}
