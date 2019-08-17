namespace Sfa.Das.Sas.Infrastructure.Mapping
{
    using Sfa.Das.FatApi.Client.Model;
    using Sfa.Das.Sas.Core.Domain;

    public interface IAssessmentOrganisationMapping
    {
        AssessmentOrganisation Map(Organisation document);
    }
}