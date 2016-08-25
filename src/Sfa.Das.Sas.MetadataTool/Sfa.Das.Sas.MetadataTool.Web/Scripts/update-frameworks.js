(function () {
    "use strict";

    var renderKeywords = function () {
        var jsons = JSON.parse($("#keywords-container").attr("data-json"));

        var source = $("#entry-template").html();
        var template = Handlebars.compile(source);

        var html = jsons.map(function (x) {
            var context = { keyword: x };
            return template(context);

        });
        $('#keywords-container').html(html);
        deleteKeywordEvent();
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
        deleteJobRoleEvent();
    }

    var deleteKeywordEvent = function () {
        $('#keywords-container .delete').on('click', function (e) {
            var val = $(this).attr('data-value');
            var values = JSON.parse($('#keywords-container').attr('data-json'));

            var newValues = values.filter(function (x) { return x !== val });

            $('#keywords-container').attr('data-json', JSON.stringify(newValues));

            renderKeywords();
        });
    }

    var deleteJobRoleEvent = function () {
        $('#jobrole-container .delete').on('click', function (e) {

            var val = $(this).attr('data-value');
            var jsons = JSON.parse($("#jobrole-container").attr("data-json"));

            var selectedJobRole = jsons[val];

            var newValues = jsons.filter(function (x) { return x !== selectedJobRole });

            $('#jobrole-container').attr('data-json', JSON.stringify(newValues));

            renderJobroles();
        });
    }

    // Add keyword
    $('#keyword-input').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();

            var jsons = JSON.parse($("#keywords-container").attr("data-json"));

            
            var newKeyword= $('#keyword-input')[0].value;
            //jsons.push({ Keyword: newKeyword });
            jsons.push( newKeyword );

            $('#keywords-container').attr('data-json', JSON.stringify(jsons));

            renderKeywords();

            deleteKeywordEvent();
            $('#keyword-input')[0].value = "";
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

    $('#jobroles-description').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();

            var jsons = JSON.parse($("#jobrole-container").attr("data-json"));

            var title = $('#jobroles-title')[0].value;
            var description = $('#jobroles-description')[0].value;
            jsons.push({ Title: title, Description: description });

            $('#jobrole-container').attr('data-json', JSON.stringify(jsons));

            renderJobroles();

            $('#jobroles-title')[0].value = "";
            $('#jobroles-description')[0].value = "";

            deleteJobRoleEvent();
            return false;
        }
    });


    // Init
    deleteJobRoleEvent();
    deleteKeywordEvent();
    renderKeywords();
    renderJobroles();

})(this);