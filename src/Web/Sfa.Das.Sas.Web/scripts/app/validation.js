var SearchAndShortlist = SearchAndShortlist || {};
(function (validation) {
    var re = /^[A-Z]{1,2}[0-9][A-Z0-9]?\s?[0-9][ABD-HJLNP-UW-Z]{2}$/i;

    validation.validatePostcode = function (postcode) {
        return re.test(postcode);
    };

    validation.init = function () {
        $('form.postcode-form').on('submit', function (e) {
            var postCode = $(this).find('.postcode-search-box').val().trim(),
                $txtPostCode = $('#search-box'),
                rbLevyPayer = document.getElementById('levyPaying'),
                rbNonLevyPayer = document.getElementById('notLevyPaying');

            $('.form-group').removeClass('error');

            if (!validation.validatePostcode(postCode)) {

                e.preventDefault();

                $txtPostCode.parent().addClass('error');

                postCode = postCode.toUpperCase().trim().replace(/\s/g, "");

                if (postCode.length > 4 && postCode.length < 8) {
                    var splitAt = postCode.length - 3;
                    postCode = [postCode.slice(0, splitAt), " ", postCode.slice(splitAt)].join("");
                }

                $('.postcode-search-box').val(postCode);
            }

            else if (rbLevyPayer.checked === false && rbNonLevyPayer.checked === false) {

                $(rbLevyPayer).closest('.form-group').addClass('error');
                $('.form-elements').addClass("error");
                e.preventDefault();

            }
        });
    };

    validation.init();
    
}(SearchAndShortlist.validation = {}));