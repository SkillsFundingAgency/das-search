namespace Sfa.Das.Sas.ApplicationServices.Validators
{
    using FluentValidation;

    using Sfa.Das.Sas.ApplicationServices.Queries;

    public sealed class ProviderDetailQueryValidator : AbstractValidator<ProviderDetailQuery>
    {
        public ProviderDetailQueryValidator(IValidation validation)
        {
            RuleFor(criteria => criteria.Ukprn).GreaterThan(0).WithErrorCode(ValidationCodes.InvalidInput);
            RuleFor(criteria => criteria.LocationId).GreaterThan(0).WithErrorCode(ValidationCodes.InvalidInput);
        }
    }
}