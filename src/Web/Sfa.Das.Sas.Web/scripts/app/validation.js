var SearchAndShortlist = SearchAndShortlist || {};
(function (validation) {
    var re= /^[A-Z]{1,2}[0-9][A-Z0-9]?\s?[0-9][ABD-HJLNP-UW-Z]{2}$/i;

    validation.ValidatePostcode = function(postcode) {
        return re.test(postcode);
    }

    validation.init = function() {
        $('form.postcode-form').on('submit', function (e) {
            var postCode = $(this).find('.postcode-search-box').val().trim();
            if (!validation.ValidatePostcode(postCode)) {
                $('.form-group').addClass("error");
                e.preventDefault();
            }
        });
    };
    validation.init();

}(SearchAndShortlist.validation = {}));