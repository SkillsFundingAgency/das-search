var SearchAndShortlist = SearchAndShortlist || {};
(function (provider)
{
    provider.AddShortlistRequest = function(shortlistLink)
    {
        var apprenticeship =
        {
            providerUkprn: shortlistLink.attr("data-provider"),
            apprenticeshipId: shortlistLink.attr("data-apprenticeship"),
            locationId: shortlistLink.attr("data-location"),
            type: shortlistLink.attr("data-apprenticeship-type")
        };

        if (apprenticeship.type === "Standard")
        {
            SearchAndShortlist.shortlist.AddStandardProvider(
                apprenticeship.providerUkprn,
                apprenticeship.apprenticeshipId,
                apprenticeship.locationId);
        } else
        {
            SearchAndShortlist.shortlist.AddFrameworkProvider(
                apprenticeship.providerUkprn,
                apprenticeship.apprenticeshipId,
                apprenticeship.locationId);
        }
    };

    provider.RemoveShortlistRequest = function(shortlistLink)
    {
        var apprenticeship =
        {
            providerUkprn: shortlistLink.attr("data-provider"),
            apprenticeshipId: shortlistLink.attr("data-apprenticeship"),
            locationId: shortlistLink.attr("data-location"),
            type: shortlistLink.attr("data-apprenticeship-type")
        };

        if (apprenticeship.type === "Standard")
        {
            SearchAndShortlist.shortlist.RemoveStandardProvider(
                apprenticeship.providerUkprn,
                apprenticeship.apprenticeshipId,
                apprenticeship.locationId);
        } else
        {
            SearchAndShortlist.shortlist.RemoveFrameworkProvider(
                apprenticeship.providerUkprn,
                apprenticeship.apprenticeshipId,
                apprenticeship.locationId);
        }
    };

    provider.init = function()
    {
        $(".provider-shortlist-link").on("click", function (e)
        {
            e.preventDefault();
            var $this = $(this);

            if ($this.attr("data-action") === "add")
            {
                provider.AddShortlistRequest($(this));
                $(".provider-shortlist-link").attr("data-action", "remove");
                $(".provider-shortlist-link").html("Remove this training provider");
            }
            else if ($this.attr("data-action") === "remove")
            {
                provider.RemoveShortlistRequest($(this));
                $(".provider-shortlist-link").attr("data-action", "add");
                $(".provider-shortlist-link").html("Shortlist this training provider");
            };
        });
    };

   provider.init();

}(SearchAndShortlist.provider = {}));