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
                errorSummary = $('#error-summary'),
                errorLink = $('#error-postcode-link');

            // Remove any errors
            $('.form-group').removeClass('error');
            errorSummary.addClass('hidden');

            if ( !validation.validatePostcode(postCode)) {

                // Prevent form submitting
                e.preventDefault();

                // Postcode Field
                if (!validation.validatePostcode(postCode)) {

                    var pcFieldGroup = pcField.closest('.form-group');
                    pcFieldGroup.addClass('error');
                    var $errorContainer = pcFieldGroup.find('.error-message');
                    $errorContainer.empty().append('<span id="error-js-postcode-invalid">' + pcFieldGroup.data('validation') + '</span>');

                    errorSummary.removeClass('hidden');
                    errorLink.text(pcFieldGroup.data('validation'));

                    postCode = postCode.toUpperCase().trim().replace(/\s/g, "");

                    if (postCode.length > 4 && postCode.length < 8) {
                        var splitAt = postCode.length - 3;
                        postCode = [postCode.slice(0, splitAt), " ", postCode.slice(splitAt)].join("");
                    }

                    pcField.val(postCode);
                }
            }

        });
    };

    validation.init();
    
}(SearchAndShortlist.validation = {}));