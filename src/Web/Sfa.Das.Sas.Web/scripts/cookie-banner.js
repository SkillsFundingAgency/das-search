var SearchAndShortlist = SearchAndShortlist || {};

(function (cookieBanner) {
    var cookieName = "seen_cookie_message";

    cookieBanner.init = function () {
        var cookie = Cookies.get(cookieName);

        if (cookie === undefined) {
            Cookies.set(cookieName, cookieName, { expires: 30, domain: SearchAndShortlist.appsettings.cookieDomain, HttpOnly: SearchAndShortlist.appsettings.cookieSecure, path: '/' });
        } else {
            $('#global-cookie-message').hide();
        }
    };

    cookieBanner.init();

}(SearchAndShortlist.CookieBanner = {}));