namespace Sfa.Das.Sas.Core.Domain.Model
{
    public sealed class Provider
    {
        public int UkPrn { get; set; }

        public bool IsHigherEducationInstitute { get; set; }

        public string Name { get; set; }

        public string LegalName { get; set; }

        public bool NationalProvider { get; set; }

        public ContactInformation ContactInformation { get; set; }

        public bool HasNonLevyContract { get; set; }

        public bool HasParentCompanyGuarantee { get; set; }

        public bool IsNew { get; set; }
    }
}
