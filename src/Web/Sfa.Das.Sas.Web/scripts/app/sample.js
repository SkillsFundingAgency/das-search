var MyTest = MyTest || {};
(function (appsettings) {

    function undefinedIfTemplate(value) {
        if (value.substr(0, 2) === String.fromCharCode(35) + String.fromCharCode(123)) {
            return undefined;
        }
        return value;
    }

    appsettings.cookieDomain = undefinedIfTemplate("#{cookie.domain}") || "localhost";

    appsettings.cookieSecure = undefinedIfTemplate("#{cookie.secure}") || "false";

}(MyTest.appsettings = {}));