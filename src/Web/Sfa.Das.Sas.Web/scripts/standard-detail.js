var SearchAndShortlist = SearchAndShortlist || {};
(function (standard) {

    standard.CookieName = 'das_shortlist_standards';

    standard.Add = function(id) {
        var value = Cookies.get(standard.CookieName);
        if (!value || value.length === 0) {
            Cookies.set(standard.CookieName, '' + id, { expires: 365, domain: SearchAndShortlist.appsettings.cookieDomain, HttpOnly: SearchAndShortlist.appsettings.cookieSecure, path: '/' });
        } else {
            var array = value.split('|');
            var index = array.indexOf(id);
            if (index < 0) {
                array.push(id);
            }
            Cookies.set(standard.CookieName, array.join('|'), { expires: 365, domain: SearchAndShortlist.appsettings.cookieDomain, HttpOnly: SearchAndShortlist.appsettings.cookieSecure, path: '/' });
        }
    };

    standard.Remove = function (id) {
        var value = Cookies.get(standard.CookieName);
        if (value && value.length !== 0) {
            var array = value.split('|');
            var index = array.indexOf(id);
            if (index >= 0) {
                array.splice(index, 1);
            }
            Cookies.set(standard.CookieName, array.join('|'), { expires: 365, domain: SearchAndShortlist.appsettings.cookieDomain, HttpOnly: SearchAndShortlist.appsettings.cookieSecure, path: '/' });
        }
    }


    standard.init = function () {
        $('.shortlist-link').on('click', function (e) {
            var $this = $(this);
            if ($this.attr('data-action') === 'add') {
                standard.Add($(this).attr('data-standard'));
                $('.shortlist-link').attr('data-action', 'remove');
                $('.shortlist-link').html('Remove from shortlist');
            } else if ($this.attr('data-action') === 'remove') {
                standard.Remove($(this).attr('data-standard'));
                $('.shortlist-link').attr('data-action', 'add');
                $('.shortlist-link').html('Shortlist apprenticeship');
            };
            e.preventDefault();
        });
    };

    standard.init();

}(SearchAndShortlist.standard = {}));