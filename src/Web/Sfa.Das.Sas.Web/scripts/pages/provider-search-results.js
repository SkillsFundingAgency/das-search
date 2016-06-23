var SearchAndShortlist = SearchAndShortlist || {};
(function (providerSearchResult)
{
    providerSearchResult.init = function ()
    {
        $(".provider-search-shortlist-link").on("click", function (e) {
            e.preventDefault();
            var $this = $(this);

            if ($this.attr("data-action") === "add") {
                SearchAndShortlist.provider.AddShortlistRequest($(this));
                $(this).attr("data-action", "remove");
                $(this).html("Remove");
            }
            else if ($this.attr("data-action") === "remove") {
                SearchAndShortlist.provider.RemoveShortlistRequest($(this));
                $(this).attr("data-action", "add");
                $(this).html("Shortlist");
            };
        });
    };

    providerSearchResult.init();

}(SearchAndShortlist.providerSearchResult = {}));