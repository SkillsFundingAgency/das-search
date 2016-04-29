var SearchAndShortlist = SearchAndShortlist || {};
(function (validation) {

    validation.init = function() {
        $('form.postcode-form').on('submit', function (e) {
            if ($(this).find('.postcode-search-box').val().trim() === "") {
                $('.form-group').addClass("error");
                e.preventDefault();
            }
        });
    };

    validation.init();

}(SearchAndShortlist.validation = {}));