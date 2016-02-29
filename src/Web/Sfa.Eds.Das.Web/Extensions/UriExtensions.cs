namespace Sfa.Eds.Das.Web.Extensions
{
    using System;
    using System.Globalization;
    using Sfa.Eds.Das.Web.ViewModels;

    public static class UriExtensions
    {

        public static LinkViewModel GetSearchResultUrl(this Uri urlReferrer, string action)
        {
            if (urlReferrer != null && urlReferrer.OriginalString.ToLower(CultureInfo.CurrentCulture).Contains("?keywords"))
            {
                return new LinkViewModel { Title = "Results", Url = urlReferrer.OriginalString };
            }

            return new LinkViewModel { Title = "Back to search page", Url = action };
        }
    }
}