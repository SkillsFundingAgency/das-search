var SearchAndShortlist = SearchAndShortlist || {};
(function (analytics) {

    var url = window.location.href.toString().split(window.location.host)[1];

    var $t = function () {
        return {
            exists: function (s) {
                return ($(s).length > 0) ? true : false;
            }
        };
    }();

    analytics.pushEvent = function (category, text) {
        ga("send", "event", {
            "eventCategory": category,
            "eventAction": "Click",
            "eventLabel": text
        });
    },

    analytics.init = function () {

        $("a[href^=mailto]").on("click", function () {
            analytics.pushEvent("email", "");
        });

        $("a.course-link").on("click", function () {
            analytics.pushEvent("link", "Provider website");
        });

        $("a.contact-link").on("click", function () {
            analytics.pushEvent("link", "Provider contact page");
        });
    };

    analytics.init();

}(SearchAndShortlist.analytics = {}));

(function () {

}).call(this);