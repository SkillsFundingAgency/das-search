namespace Sfa.Das.Sas.Web.Common
{
    using System.Text.RegularExpressions;

    public static class Validation
    {
        public static bool ValidatePostcode(string postCode)
        {
            var match = Regex.Match(postCode ?? string.Empty, @"^[A-Z]{1,2}[0-9][A-Z0-9]?\s?[0-9][ABD-HJLNP-UW-Z]{2}$", RegexOptions.IgnoreCase);
            return match.Success;
        }
    }
}