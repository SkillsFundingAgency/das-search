var SearchAndShortlist = SearchAndShortlist || {};
(function (appsettings) {

    function undefinedIfTemplate(value) {
        if (value.substr(0, 2) === "#{") {
            return;
        }
        return value;
    }

    appsettings.cookieDomain = undefinedIfTemplate("#{cookie.domain}") || "localhost";

    appsettings.cookieSecure = undefinedIfTemplate("#{cookie.secure}") || "false";


    appsettings.init = function () {

    };

    appsettings.init();

}(SearchAndShortlist.appsettings = {}));