var SearchAndShortlist = SearchAndShortlist || {};
(function(cookieStore) {

    cookieStore.GetCookie = function (name) {
        var cookie = new SearchAndShortlist.Cookie(name);

        var cookieString = Cookies.get(name);

        if (cookieString && cookieString.length !== 0) {
            cookie.PopulateFromString(cookieString);
        }

        return cookie;
    };

    cookieStore.SaveCookie = function(cookie) {
        Cookies.set(cookie.Name,
            cookie.ToString(),
            {
                expires: 365,
                domain: SearchAndShortlist.appsettings.cookieDomain,
                secure: SearchAndShortlist.appsettings.cookieSecure === "true",
                httponly: false,
                path: "/"
            });
    };

}(SearchAndShortlist.CookieStore = {}));

