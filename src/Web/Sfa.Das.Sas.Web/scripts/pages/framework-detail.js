var SearchAndShortlist = SearchAndShortlist || {};
(function (framework) {

    framework.init = function() {

        $(".showmore").removeClass("hidden");
        $(".default-hidden").hide();

        $(".showmore").on("click", function (e) {
            e.preventDefault();
            if ($(".default-hidden").is(":visible"))
            {
                $(".showmore").text("Show more");
                $(".default-hidden").hide();
            }
            else
            {
                $(".showmore").text("Show less");
                $(".default-hidden").show();
            }
        });
    };
    framework.init();

}(SearchAndShortlist.framework = {}));