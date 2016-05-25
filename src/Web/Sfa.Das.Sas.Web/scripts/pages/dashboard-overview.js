var SearchAndShortlist = SearchAndShortlist || {};
(function (dashboard) {

    dashboard.RemoveElement = function(element) {

        if (element) {
            element.remove();
        }
    };

    dashboard.init = function () {
        $('.standard-delete-link').on('click', function (e) {
            e.preventDefault();
            
            SearchAndShortlist.standard.Remove($(this).attr('data-apprenticeship'));

            var standardRow = $(this).closest(".standard-item");

            dashboard.RemoveElement(standardRow);
        });

        $('.provider-delete-link').on('click', function (e) {
            e.preventDefault();
            
            SearchAndShortlist.provider.Remove(
                $(this).attr('data-provider'),
                $(this).attr('data-apprenticeship'),
                $(this).attr('data-location'));

            var providerRow = $(this).closest(".provider-item");

            dashboard.RemoveElement(providerRow);
        });
    };

    dashboard.init();

}(SearchAndShortlist.dashboard = {}));