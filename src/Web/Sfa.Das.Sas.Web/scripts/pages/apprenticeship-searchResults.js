var SearchAndShortlist = SearchAndShortlist || {};
(function (apprenticeship) {

    apprenticeship.StandardCookieName = 'das_shortlist_standards';
    apprenticeship.FrameworkCookieName = 'das_shortlist_frameworks';

    apprenticeship.AddStandard = function (id) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(apprenticeship.StandardCookieName);

        if (cookie) {
            cookie.AddSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    apprenticeship.RemoveStandard = function(id) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(apprenticeship.StandardCookieName);

        if (cookie) {
            cookie.RemoveSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    apprenticeship.AddFramework = function (id) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(apprenticeship.FrameworkCookieName);

        if (cookie) {
            cookie.AddSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    apprenticeship.RemoveFramework = function (id) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(apprenticeship.FrameworkCookieName);

        if (cookie) {
            cookie.RemoveSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };


    apprenticeship.init = function () {
        $('.standard-search-shortlist-link').on('click', function (e) {
            e.preventDefault();
            var $this = $(this);
            if ($this.attr('data-action') === 'add') {
                apprenticeship.AddStandard($this.attr('data-standard'));
                $this.attr('data-action', 'remove');
                $this.html('Remove');;
            } else if ($this.attr('data-action') === 'remove') {
                apprenticeship.RemoveStandard($(this).attr('data-standard'));
                $this.attr('data-action', 'add')
                $this.html('Shortlist');
            };
        });

        $('.framework-search-shortlist-link').on('click', function (e) {
            e.preventDefault();
            var $this = $(this);
            if ($this.attr('data-action') === 'add') {
                apprenticeship.AddFramework($this.attr('data-framework'));
                $this.attr('data-action', 'remove');
                $this.html('Remove');;
            } else if ($this.attr('data-action') === 'remove') {
                apprenticeship.RemoveFramework($this.attr('data-framework'));
                $this.attr('data-action', 'add');
                $this.html('Shortlist');
            };
        });
    };

    apprenticeship.init();

}(SearchAndShortlist.standard = {}));