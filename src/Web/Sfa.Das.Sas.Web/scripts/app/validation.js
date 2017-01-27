var SearchAndShortlist = SearchAndShortlist || {};
(function (validation) {
    var re = /^[A-Z]{1,2}[0-9][A-Z0-9]?\s?[0-9][ABD-HJLNP-UW-Z]{2}$/i;

    validation.validatePostcode = function (postcode) {
        return re.test(postcode);
    };

    validation.init = function () {
        $('form.postcode-form').on('submit', function (e) {
            var pcField = $('#search-box'),
                postCode = pcField.val().trim(),
                rbLevyPayer = $('#levyPaying'),
                rbNonLevyPayer = $('#notLevyPaying');

            // Remove any errors
            $('.form-group').removeClass('error');

            if ( !validation.validatePostcode(postCode) ||
                (!rbLevyPayer.prop('checked') && !rbNonLevyPayer.prop('checked')) ) {

                // Prevent form submitting
                e.preventDefault();

                // Postcode Field
                if (!validation.validatePostcode(postCode)) {

                    var pcFieldGroup = pcField.closest('.form-group');
                    pcFieldGroup.addClass('error');
                    pcFieldGroup.find('.error-message').text(pcFieldGroup.data('validation'));

                    postCode = postCode.toUpperCase().trim().replace(/\s/g, "");

                    if (postCode.length > 4 && postCode.length < 8) {
                        var splitAt = postCode.length - 3;
                        postCode = [postCode.slice(0, splitAt), " ", postCode.slice(splitAt)].join("");
                    }

                    pcField.val(postCode);
                }

                // Radio Buttons
                if (!rbLevyPayer.prop('checked') && !rbNonLevyPayer.prop('checked')) {
                    var rbFieldGroup = rbLevyPayer.closest('.form-group');
                    rbFieldGroup.closest('.form-group').addClass('error');
                    rbFieldGroup.find('.error-message').text(rbFieldGroup.data('validation'));
                }
            }

        });
    };

    validation.init();
    
}(SearchAndShortlist.validation = {}));