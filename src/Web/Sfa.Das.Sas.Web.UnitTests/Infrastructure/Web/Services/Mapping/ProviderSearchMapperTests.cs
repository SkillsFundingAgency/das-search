using FluentAssertions;

using NUnit.Framework;

using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Web.Services.MappingActions.Helpers;

namespace Sfa.Das.Sas.Web.UnitTests.Infrastructure.Web.Services.Mapping
{
    [TestFixture]
    public class ProviderSearchMapperTests
    {

        [TestCase(ProviderSearchResponseCodes.LocationServiceUnavailable, "Sorry, postcode search not working, please try again later")]
        [TestCase(ProviderSearchResponseCodes.PostCodeTerminated, "Sorry, this postcode is no longer valid")]
        [TestCase(ProviderSearchResponseCodes.PostCodeInvalidFormat, "You must enter a full and valid postcode")]
        [TestCase(ProviderSearchResponseCodes.WalesPostcode, "The postcode entered is not in England. Information about apprenticeships in <a href=\"https://businesswales.gov.wales/skillsgateway/apprenticeships\">Wales</a>")]
        [TestCase(ProviderSearchResponseCodes.NorthernIrelandPostcode, "The postcode entered is not in England. Information about apprenticeships in <a href=\"https://www.nibusinessinfo.co.uk/content/apprenticeships-employers\">Northern Ireland</a>")]
        [TestCase(ProviderSearchResponseCodes.ScotlandPostcode, "The postcode entered is not in England. Information about apprenticeships in <a href=\"https://www.apprenticeships.scot/\">Scotland</a>")]
        [TestCase(ProviderSearchResponseCodes.Success, "")]
        [TestCase(ProviderSearchResponseCodes.InvalidApprenticeshipId, "")]
        [TestCase(ProviderSearchResponseCodes.ServerError, "")]
        [TestCase(ProviderSearchResponseCodes.PageNumberOutOfUpperBound, "")]
        public void Sleep(ProviderSearchResponseCodes input, string expected)
        {
            ProviderSearchMapper.CreateErrorMessage(input).Should().Be(expected);
        }
    }
}
