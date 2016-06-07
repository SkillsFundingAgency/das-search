var SearchAndShortlist = SearchAndShortlist || {};
(function (standard) {

    standard.CookieName = 'das_shortlist_standards';

    standard.Add = function (id) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(standard.CookieName);

        if (cookie) {
            cookie.AddSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    standard.Remove = function (id) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(standard.CookieName);

        if (cookie) {
            cookie.RemoveSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    }


    standard.init = function () {
        $('.standard-shortlist-link').on('click', function (e) {
            e.preventDefault();
            var $this = $(this);
            if ($this.attr('data-action') === 'add') {
                standard.Add($(this).attr('data-standard'));
                $('.standard-shortlist-link').attr('data-action', 'remove');
                $('.standard-shortlist-link').html('Remove from shortlist');
            } else if ($this.attr('data-action') === 'remove') {
                standard.Remove($(this).attr('data-standard'));
                $('.standard-shortlist-link').attr('data-action', 'add');
                $('.standard-shortlist-link').html('Shortlist apprenticeship');
            };
           
        });
    };

    standard.init();

}(SearchAndShortlist.standard = {}));