var SearchAndShortlist = SearchAndShortlist || {};
(function (standard) {

    standard.CookieName = 'standards_shortlist';

    standard.getCookie = function(name) {
        var value = "; " + document.cookie;
        var parts = value.split("; " + name + "=");
        if (parts.length == 2)
            return parts.pop().split(";").shift();
    };

    standard.setCookie = function(name, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var cValue = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
        document.cookie = name + "=" + cValue;
    };

    standard.Add = function(id) {
        console.log('Adding standard ' + id);
        var value = standard.getCookie(standard.CookieName);
        if (!value || value.length === 0) {
            // create a cookie
        } else {
            // add id to cookie
            var array = value.split(',');
            console.log(array);
        }
    };

    standard.Remove = function (id) {
        console.log('Removing standard ' + id);
    }


    standard.init = function () {
        $('.shortlist-linkREMOVE').on('click', function (e) {
            e.stopPropagation();
            var value = standard.getCookie(standard.CookieName);
            console.log(value);
            if ($(this).attr('data-action') === 'add') {
                standard.Add($(this).attr('data-standard'));
            } 
            if ($(this).attr('data-action') === 'remove') {
                standard.Remove($(this).attr('data-standard'));
            }

            return 0;
            if ($(this).find('.postcode-search-box').val().trim() === "") {
                $('.form-group').addClass("error");
                e.preventDefault();
            }
        });
    };

    standard.init();

}(SearchAndShortlist.standard = {}));