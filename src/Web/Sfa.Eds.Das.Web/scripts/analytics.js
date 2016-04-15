var SearchAndShortlist = SearchAndShortlist || {};
(function (analytics) {

    analytics.pushEventValue = function (category, text, value) {
        ga("send", "event", {
            "eventCategory": category,
            "eventAction": "Click",
            "eventLabel": text,
            "eventValue": value
        });
    },

    analytics.pushEvent = function (category, text) {
        ga("send", "event", {
            "eventCategory": category,
            "eventAction": "Click",
            "eventLabel": text
        });
    },

    analytics.init = function () {

        $("#standard-results .result a:lt(3)").on("click", function () {
            analytics.pushEvent("Apprenticeship Search", "top3");
        });

        $("#standard-results .result a:gt(2)").on("click", function () {
            analytics.pushEvent("Apprenticeship Search", "result");
        });

        $("#start-button").on("click", function () {
            analytics.pushEvent("button", "start");
        });

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