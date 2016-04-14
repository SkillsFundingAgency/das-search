var SearchAndShortlist = SearchAndShortlist || {};
(function (analytics) {

    analytics.pushEvent = function (category, text) {
        ga("send", "event", {
            "eventCategory": category,
            "eventAction": "Click",
            "eventLabel": text
        });
    },

    analytics.init = function () {

        $("data-list a[href^=mailto], .data-list a.course-link, .data-list a.contact-link").on("click", function () {
            analytics.pushEvent("contact", "Provider page");
        });

        $(".data-list a[href^=mailto]").on("click", function () {
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