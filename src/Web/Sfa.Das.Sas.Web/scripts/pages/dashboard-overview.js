var SearchAndShortlist = SearchAndShortlist || {};
(function (dashboard)
{
    dashboard.RemoveElement = function (element)
    {
        if (element)
        {
            element.remove();
        }

        dashboard.RefreshShortListDisplay = function ()
        {
            if ($(".apprenticeship-items .apprenticeship-item").length === 0)
            {
                $(".shortlist").hide();
                $("#empty-shortlist-message").removeClass("hidden");
            }
        };
    };

    dashboard.init = function ()
    {
        $(".delete-link").on("click", function (e)
        {
            e.preventDefault();
            if ($(this).attr("data-apprenticeship-type") === "RemoveStandard") {
                SearchAndShortlist.standard.Remove($(this).attr("data-apprenticeship"));
            }
            else {
                SearchAndShortlist.framework.Remove($(this).attr("data-apprenticeship"));
            }

            var standardRow = $(this).closest(".apprenticeship-item");

            dashboard.RemoveElement(standardRow);
            dashboard.RefreshShortListDisplay();
        });

        $(".provider-delete-link").on("click", function (e)
        {
            e.preventDefault();

            var apprenticeship =
            {
                providerId: $(this).attr("data-provider"),
                apprenticeshipId: $(this).attr("data-apprenticeship"),
                locationId: $(this).attr("data-location"),
                type: $(this).attr("data-apprenticeship-type")
            };

            if (apprenticeship.type === "Standard")
            {
                SearchAndShortlist.shortlist.RemoveStandardProvider(
                 apprenticeship.providerId,
                 apprenticeship.apprenticeshipId,
                 apprenticeship.locationId);
            }
            else
            {
                SearchAndShortlist.shortlist.RemoveFrameworkProvider(
                  apprenticeship.providerId,
                  apprenticeship.apprenticeshipId,
                  apprenticeship.locationId);
            }

            var providers = $(this).closest(".providers");
            var providerRow = $(this).closest(".provider-item");
            dashboard.RemoveElement(providerRow);

            if (providers.find(".provider-item").length === 0)
            {
                providers.hide();
            }

            dashboard.RefreshShortListDisplay();
        });
    };

    dashboard.init();

}(SearchAndShortlist.dashboard = {}));