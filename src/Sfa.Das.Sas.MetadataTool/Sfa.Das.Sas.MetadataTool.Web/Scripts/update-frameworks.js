(function () {
    "use strict";

    var deleteGeneric = function (containerId, render) {
        var containerSelector = '#' + containerId;
        $(containerSelector + ' .delete').on('click', function (e) {

            var val = $(this).attr('data-value');
            var jsons = JSON.parse($(containerSelector).attr("data-json"));

            var selectedItem = jsons[val];

            var newValues = jsons.filter(function (x) { return x !== selectedItem });

            $(containerSelector).attr('data-json', JSON.stringify(newValues));

            render();
        });
    }

    var renderKeywords = function () {
        var jsons = JSON.parse($("#keywords-container").attr("data-json")).filter(function(x) { return x !== ""});

        var keywordTemplate = Handlebars.compile($("#entry-template").html());

        var html = jsons.map(function (x, i) {
            var context = { keyword: x, index: i};
            return keywordTemplate(context);

        });
        $('#keywords-container').html(html);
        deleteGeneric('keywords-container', renderKeywords);
    }
    
    var renderJobroles = function () {
        var jsons = JSON.parse($("#jobrole-container").attr("data-json"));

        var source = $("#jobrole-template").html();
        var template = Handlebars.compile(source);

        var html = jsons.map(function (x, i) {
            var context = { title: x.Title, description: x.Description, index: i };
            return template(context);

        });

        $('#jobrole-container').html(html);
        deleteGeneric('jobrole-container', renderJobroles);
    }

    var readValueFromInput = function(fieldId) {
        var val = $(fieldId)[0].value;
        $(fieldId)[0].value = "";
        return val;
    }

    // Add keyword
    $('#keyword-input').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();

            var jsons = JSON.parse($("#keywords-container").attr("data-json"));
            var newKeyword = readValueFromInput('#keyword-input');
            jsons.push( newKeyword );

            $('#keywords-container').attr('data-json', JSON.stringify(jsons));
            renderKeywords();
            return false;
        }
    });

    // Add job role
    $('#jobroles-description').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();

            var jsons = JSON.parse($("#jobrole-container").attr("data-json"));

            var title = readValueFromInput('#jobroles-title');
            var description = readValueFromInput('#jobroles-description');
            jsons.push({ Title: title, Description: description });

            $('#jobrole-container').attr('data-json', JSON.stringify(jsons));

            renderJobroles();
            return false;
        }
    });

    $('#jobroles-title').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            $("#jobroles-description").focus();
            return false;
        }
        return true;
    });


    // Init
    deleteGeneric('jobrole-container', renderJobroles);
    deleteGeneric('keywords-container', renderKeywords);
    renderKeywords();
    renderJobroles();

})(this);