var SearchAndShortlist = SearchAndShortlist || {};
(function (shortlist)
{
    shortlist.StandardCookieName = "das_shortlist_standards";
    shortlist.FrameworkCookieName = "das_shortlist_frameworks";

    shortlist.AddStandardProvider = function (providerId, apprenticeshipId, locationId)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(shortlist.StandardCookieName);

        if (cookie)
        {
            cookie.AddSubKeyValue(apprenticeshipId, providerId + "-" + locationId);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    shortlist.AddFrameworkProvider = function (providerId, apprenticeshipId, locationId)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(shortlist.FrameworkCookieName);

        if (cookie) {
            cookie.AddSubKeyValue(apprenticeshipId, providerId + "-" + locationId);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    shortlist.RemoveStandardProvider = function (providerId, apprenticeshipId, locationId)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(shortlist.StandardCookieName);

        if (cookie) {
            cookie.RemoveSubKeyValue(apprenticeshipId, providerId + "-" + locationId);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    shortlist.RemoveFrameworkProvider = function (providerId, apprenticeshipId, locationId)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(shortlist.FrameworkCookieName);

        if (cookie) {
            cookie.RemoveSubKeyValue(apprenticeshipId, providerId + "-" + locationId);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    shortlist.AddStandard = function (id)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(shortlist.StandardCookieName);

        if (cookie) {
            cookie.AddSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    shortlist.RemoveStandard = function (id)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(shortlist.StandardCookieName);

        if (cookie) {
            cookie.RemoveSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    shortlist.AddFramework = function (id)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(shortlist.FrameworkCookieName);

        if (cookie) {
            cookie.AddSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    shortlist.RemoveFramework = function (id)
    {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(shortlist.FrameworkCookieName);

        if (cookie) {
            cookie.RemoveSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

}(SearchAndShortlist.shortlist = {}));