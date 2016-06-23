var SearchAndShortlist = SearchAndShortlist || {};
(function (apprenticeship)
{
    apprenticeship.init = function () {
        $(".standard-search-shortlist-link").on("click", function (e) {
            e.preventDefault();
            var $this = $(this);
            if ($this.attr("data-action") === "add") {
                SearchAndShortlist.shortlist.AddStandard($this.attr("data-standard"));
                $this.attr("data-action", "remove");
                $this.html('Remove');
            } else if ($this.attr("data-action") === "remove") {
                SearchAndShortlist.shortlist.RemoveStandard($(this).attr("data-standard"));
                $this.attr("data-action", "add")
                $this.html('Shortlist');
            };
        });

        $(".framework-search-shortlist-link").on("click", function (e) {
            e.preventDefault();
            var $this = $(this);
            if ($this.attr("data-action") === "add") {
                SearchAndShortlist.shortlist.AddFramework($this.attr("data-framework"));
                $this.attr("data-action", "remove");
                $this.html('Remove');
            } else if ($this.attr("data-action") === "remove") {
                SearchAndShortlist.shortlist.RemoveFramework($this.attr("data-framework"));
                $this.attr("data-action", "add");
                $this.html('Shortlist');
            };
        });
    };

    apprenticeship.init();

}(SearchAndShortlist.Apprenticeship = {}));