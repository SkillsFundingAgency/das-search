using Sfa.Das.Sas.ApplicationServices.Responses;

namespace Sfa.Das.Sas.Web.Services.MappingActions.Helpers
{
    public static class ProviderSearchMapper
    {
        public static string CreateErrorMessage(ProviderSearchResponseCodes? statusCode)
        {
            var postCodeNotInEngland = "The postcode entered is not in England. Information about apprenticeships in";
            switch (statusCode)
            {
                case ProviderSearchResponseCodes.LocationServiceUnavailable:
                    return "Sorry, postcode search not working, please try again later";
                case ProviderSearchResponseCodes.PostCodeTerminated:
                    return "This postcode is no longer valid; please search again";
                case ProviderSearchResponseCodes.PostCodeInvalidFormat:
                    return "You must enter a full and valid postcode";
                case ProviderSearchResponseCodes.WalesPostcode:
                    return $"{postCodeNotInEngland} <a href=\"https://businesswales.gov.wales/skillsgateway/apprenticeships\">Wales</a>";
                case ProviderSearchResponseCodes.NorthernIrelandPostcode:
                    return $"{postCodeNotInEngland} <a href=\"https://www.nibusinessinfo.co.uk/content/apprenticeships-employers\">Northern Ireland</a>";
                case ProviderSearchResponseCodes.ScotlandPostcode:
                    return $"{postCodeNotInEngland} <a href=\"https://www.apprenticeships.scot/\">Scotland</a>";
            }

            return string.Empty;
        }
    }
}