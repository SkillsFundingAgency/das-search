// GDS Radio buttons 
var selectionButtons = new GOVUK.SelectionButtons("label input[type='radio'], label input[type='checkbox'], section input[type='radio']");

var SearchAndShortlist = SearchAndShortlist || {};
(function ( ui ) {

    ui.init = function () {
        $('.filters .toggler').on('click', function() {
            $(this).parents('.filters').toggleClass('unfolded');
        });


        $('html.js #search-results-order select').change(function() {
            $('#search-results-order').submit();
        });

        if ($('html.js').length) {
            var width = screen.width;
            if (width < 641) {
                $('.editSearch').addClass('folded');
            }
        }

        $('html.js .editSearch h2 a').on('click', function (e) {
            var width = screen.width;
            if (width < 641) {
                $('.editSearch').toggleClass('folded');
            }
            e.preventDefault();
        });

    };

    ui.init();

}(SearchAndShortlist.ui = {}));


var container = document.querySelector('#apprenticeships-container'),
    $container = $(container);

if (container) {

    $container.empty();
    var getSuggestions = function (query, updateResults) {

        var results = [];
        $.ajax({
            url: $container.data('api-url'),
            dataType: 'json',
            data: { searchString: query }

        }).done(function (data) {
            results = data.Results.map(function (r) {
                return r.Title;
            });
            updateResults(results);
        });
    };

    accessibleAutocomplete({
        element: container,
        id: 'keywords',
        name: 'Keywords',
        displayMenu: 'overlay',
        showNoOptionsFound: false,
        source: getSuggestions,
        defaultValue: $container.data('search-term')
    });
}