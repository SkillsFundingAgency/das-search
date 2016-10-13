using FluentValidation.TestHelper;
using NUnit.Framework;
using Sfa.Das.Sas.ApplicationServices.Validators;

namespace Sfa.Das.Sas.Web.UnitTests.Application.Validators
{
    [TestFixture]
    public sealed class ProviderSearchQueryValidatorTests
    {
        private ProviderSearchQueryValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new ProviderSearchQueryValidator(new Validation());
        }

        [TestCase("")]
        [TestCase("123")]
        [TestCase("YO31 1")]
        [TestCase("gfdsdf")]
        public void ShouldHaveErrorWhenPostCodeIsInvalid(string postCode)
        {
            _validator.ShouldHaveValidationErrorFor(query => query.PostCode, postCode);
        }

        public void ShouldHaveErrorWhenPostCodeIsNull()
        {
            _validator.ShouldHaveValidationErrorFor(query => query.PostCode, null as string);
        }

        [TestCase("A9 9AA")]
        [TestCase("A9A 9AA")]
        [TestCase("A99 9AA")]
        [TestCase("AA9 9AA")]
        [TestCase("AA9A 9AA")]
        [TestCase("NW67XT")]
        [TestCase("W1Y 8HE")]
        [TestCase("B721HE")]
        public void ShouldNotHaveErrorWhenPostCodeIsValid(string postCode)
        {
            _validator.ShouldNotHaveValidationErrorFor(query => query.PostCode, postCode);
        }
    }
}
