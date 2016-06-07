var SearchAndShortlist = SearchAndShortlist || {};
(function (framework) {

    framework.CookieName = 'das_shortlist_frameworks';

    framework.Add = function (id) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(framework.CookieName);

        if (cookie) {
            cookie.AddSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    };

    framework.Remove = function (id) {
        var cookie = SearchAndShortlist.CookieStore.GetCookie(framework.CookieName);

        if (cookie) {
            cookie.RemoveSubKey(id);
            SearchAndShortlist.CookieStore.SaveCookie(cookie);
        }
    }


    framework.init = function () {
        $('.framework-shortlist-link').on('click', function (e) {
            e.preventDefault();
            var $this = $(this);
            if ($this.attr('data-action') === 'add') {
                framework.Add($(this).attr('data-framework'));
                $('.framework-shortlist-link').attr('data-action', 'remove');
                $('.framework-shortlist-link').html('Remove from shortlist');
            } else if ($this.attr('data-action') === 'remove') {
                framework.Remove($(this).attr('data-framework'));
                $('.framework-shortlist-link').attr('data-action', 'add');
                $('.framework-shortlist-link').html('Shortlist apprenticeship');
            };
           
        });
    };

    framework.init();

}(SearchAndShortlist.framework = {}));