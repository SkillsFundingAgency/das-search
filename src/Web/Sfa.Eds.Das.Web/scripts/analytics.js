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

        $("#standard-results .result a:lt(3)").on("click", function () {
            analytics.pushEvent("Apprenticeship Search Results", "top3");
        });

        $("#standard-results .result a:gt(2)").on("click", function () {
            analytics.pushEvent("Apprenticeship Search Results", "results");
        });

        $("#start-button").on("click", function () {
            analytics.pushEvent("Start page", "Start button");
        });

        $("data-list a[href^=mailto], .data-list a.course-link, .data-list a.contact-link").on("click", function () {
            analytics.pushEvent("Provider Details", "Contact link");
        });

        $(".data-list a[href^=mailto]").on("click", function () {
            analytics.pushEvent("Provider Details", "Email");
        });

        $("a.course-link").on("click", function () {
            analytics.pushEvent("Provider Details", "Website");
        });

        $("a.contact-link").on("click", function () {
            analytics.pushEvent("Provider Details", "Contact page");
        });
    };

    analytics.init();

}(SearchAndShortlist.analytics = {}));