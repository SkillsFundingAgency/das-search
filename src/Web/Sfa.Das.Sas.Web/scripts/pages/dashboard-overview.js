var SearchAndShortlist = SearchAndShortlist || {};
(function (dashboard) {

    dashboard.RemoveElement = function(element) {

        if (element) {
            element.remove();
        }

        dashboard.RefreshShortListDisplay = function() {
            if ($('.apprenticeship-items .apprenticeship-item').length === 0) {
                $('.apprenticeship-items').hide();
                $('#empty-shortlist-message').show();
            }
        }
    };

    dashboard.init = function () {
        $('.delete-link').on('click', function (e) {
            e.preventDefault();

            if ($(this).attr('data-apprenticeship-type') === "RemoveStandard") {
                SearchAndShortlist.standard.Remove($(this).attr('data-apprenticeship'));
            } else {
                SearchAndShortlist.framework.Remove($(this).attr('data-apprenticeship'));
            }

            var standardRow = $(this).closest(".apprenticeship-item");

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