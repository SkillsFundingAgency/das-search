var SearchAndShortlist = SearchAndShortlist || {};
(function (dashboard) {

    dashboard.RemoveElement = function(element) {

        if (element) {
            element.remove();
        }

        dashboard.RefreshShortListDisplay = function() {
            if ($('#shortlist .apprenticeship-item').length === 0) {
                $('#shortlist').hide();
                $('#empty-shortlist-message').show();
            }
        }
    };

    dashboard.init = function () {
        $('.standard-delete-link').on('click', function (e) {
            e.preventDefault();
            
            SearchAndShortlist.standard.Remove($(this).attr('data-apprenticeship'));

            var standardRow = $(this).closest(".standard-item");

            dashboard.RemoveElement(standardRow);

            dashboard.RefreshShortListDisplay();
        });

        $('.framework-delete-link').on('click', function (e) {
            e.preventDefault();

            SearchAndShortlist.framework.Remove($(this).attr('data-apprenticeship'));

            var standardRow = $(this).closest(".framework-item");

            dashboard.RemoveElement(standardRow);

            dashboard.RefreshShortListDisplay();

        });

        $('.provider-delete-link').on('click', function (e) {
            e.preventDefault();
            
            SearchAndShortlist.provider.Remove(
                $(this).attr('data-provider'),
                $(this).attr('data-apprenticeship'),
                $(this).attr('data-location'));

            var providerRow = $(this).closest(".provider-item");

            dashboard.RemoveElement(providerRow);

            dashboard.RefreshShortListDisplay();
        });
    };

    dashboard.init();

}(SearchAndShortlist.dashboard = {}));