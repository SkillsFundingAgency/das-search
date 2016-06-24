namespace Sfa.Das.Sas.ApplicationServices.Validators
{
    using FluentValidation;

    using Sfa.Das.Sas.ApplicationServices.Queries;

    public sealed class ProviderDetailQueryValidator : AbstractValidator<ProviderDetailQuery>
    {
        public ProviderDetailQueryValidator(IValidation validation)
        {
            RuleFor(criteria => criteria.ProviderId).Must(validation.IsPositiveNumber).WithErrorCode(ValidationCodes.InvalidInput);
            RuleFor(criteria => criteria.LocationId).Must(validation.IsPositiveNumber).WithErrorCode(ValidationCodes.InvalidInput);
        }
    }
}