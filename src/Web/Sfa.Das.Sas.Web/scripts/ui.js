
var ui = ui || {};
(function () {

    ui.init = function () {
        $('.filters .toggler').on('click', function () {
            $(this).parents('.filters').toggleClass('unfolded');
        })

        
        $('html.js #search-results-order select').change(function (e) {
            $('#search-results-order').submit();
        })

    };

    ui.init();

}());