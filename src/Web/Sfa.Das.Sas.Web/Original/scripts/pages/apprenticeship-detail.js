var SearchAndShortlist = SearchAndShortlist || {};
(function (details) {

    details.init = function () {

        $(".showmore").removeClass("hidden");
        $(".default-hidden").hide();

        $(".showmore").on("click", function (e) {
            e.preventDefault();

            var $showHideLink = $(e.target);
            var $hiddenSiblings = $showHideLink.siblings('.default-hidden');

            if ($hiddenSiblings.is(":visible"))
            {
                $showHideLink.text("Show more");
                $hiddenSiblings.hide();
            }
            else
            {
                $showHideLink.text("Show less");
                $hiddenSiblings.show();
            }
        });
    };

    details.init();

}(SearchAndShortlist.apprenticeshipDetails = {}));