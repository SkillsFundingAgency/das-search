var SearchAndShortlist = SearchAndShortlist || {};
(function (provider)
{
    provider.AddShortlistRequest = function (shortlistLink)
    {
        var apprenticeship =
        {
            providerId: shortlistLink.attr("data-provider"),
            apprenticeshipId: shortlistLink.attr("data-apprenticeship"),
            locationId: shortlistLink.attr("data-location"),
            type : shortlistLink.attr("data-apprenticeship-type")
        };
        
        if (apprenticeship.type === "Standard") {
            SearchAndShortlist.shortlist.AddStandardProvider(
                apprenticeship.providerId,
                apprenticeship.apprenticeshipId,
                apprenticeship.locationId);
        }
        else
        {
            SearchAndShortlist.shortlist.AddFrameworkProvider(
                 apprenticeship.providerId,
                apprenticeship.apprenticeshipId,
                apprenticeship.locationId);
        }
    }

    provider.RemoveShortlistRequest = function (shortlistLink)
    {
        var apprenticeship =
       {
           providerId: shortlistLink.attr("data-provider"),
           apprenticeshipId: shortlistLink.attr("data-apprenticeship"),
           locationId: shortlistLink.attr("data-location"),
           type: shortlistLink.attr("data-apprenticeship-type")
       };

        if (apprenticeship.type === "Standard") {
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
    }

    provider.init = function()
    {
        $(".provider-shortlist-link").on("click", function (e)
        {
            e.preventDefault();
            var $this = $(this);

            if ($this.attr("data-action") === "add")
            {
                provider.AddShortlistRequest($(this));
                $(this).attr("data-action", "remove");
                $(this).html("Remove this training provider");
            }
            else if ($this.attr("data-action") === "remove")
            {
                provider.RemoveShortlistRequest($(this));
                $(this).attr("data-action", "add");
                $(this).html("Shortlist this training provider");
            };
        });
    };

   provider.init();

}(SearchAndShortlist.provider = {}));