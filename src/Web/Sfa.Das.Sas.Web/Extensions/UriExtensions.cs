using System;
using System.Globalization;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Extensions
{
    public static class UriExtensions
    {
        public static LinkViewModel GetSearchResultUrl(this Uri urlReferrer, string action)
        {
            if (urlReferrer != null && urlReferrer.OriginalString.ToLower(CultureInfo.CurrentCulture).Contains("?keywords"))
            {
                return new LinkViewModel { Title = "Back", Url = urlReferrer.OriginalString };
            }

            return new LinkViewModel { Title = "Back to search page", Url = action };
        }

        public static LinkViewModel GetProviderSearchResultBackUrl(this Uri urlReferrer, string action)
        {
            if (urlReferrer != null && urlReferrer.OriginalString.ToLower(CultureInfo.CurrentCulture).Contains("?apprenticeshipid") && urlReferrer.OriginalString.ToLower(CultureInfo.CurrentCulture).Contains("&postcode"))
            {
                return new LinkViewModel { Title = "Back", Url = urlReferrer.OriginalString };
            }

            return new LinkViewModel { Title = "Back to search page", Url = action };
        }
    }
}