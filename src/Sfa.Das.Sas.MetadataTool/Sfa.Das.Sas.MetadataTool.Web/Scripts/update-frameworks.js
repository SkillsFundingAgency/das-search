(function () {
    "use strict";

    var deleteGeneric = function (containerSelector, render, templateQuery, name) {
        
        $(containerSelector).find('.delete').on('click', function (e) {

            var val = $(this).attr('data-value');
            var jsons = JSON.parse($(containerSelector).attr("data-json"));

            var selectedItem = jsons[val];

            var newValues = jsons.filter(function (x) { return x !== selectedItem });

            $(containerSelector).attr('data-json', JSON.stringify(newValues));

            render(containerSelector, templateQuery, name);
        });
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
        deleteGeneric('#jobrole-container', renderJobroles, "#jobrole-template");
    }

    var renderGeneric = function (container, templateQuery, name) {

        var jsons = JSON.parse($(container).attr("data-json"));
        var source = $(templateQuery).html();
        var template = Handlebars.compile(source);

        var html = jsons.map(function (x, i) {
            var context = { entry: x, name: name, index: i };
            return template(context);
        });

        $(container).html(html);
        deleteGeneric(container, renderGeneric, templateQuery, name);
    }

    var readValueFromInput = function(fieldId) {
        var val = $(fieldId)[0].value;
        $(fieldId)[0].value = "";
        return val;
    }

    var getJson = function (containersSelector) {
        var data = containersSelector.attr("data-json");
        var data2 = data === "" ? "[]" : data;
        return JSON.parse(data2);
    }

    var addData = function (containerSelector, entry) {
        var jsons = getJson(containerSelector);
        jsons.push(entry);
        containerSelector.attr('data-json', JSON.stringify(jsons));
    }

    // Add keyword
    $('#keyword-input').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            var container = $("#keywords-container");
            var newKeyword = readValueFromInput('#keyword-input');

            addData(container, newKeyword);

            renderGeneric(container, "#entry-template", "keywords");
            return false;
        }
    });

    // Add standard job role
    $('#jobroles-input').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            var container = $("#jobroles-container");
            var newKeyword = readValueFromInput('#jobroles-input');

            addData(container, newKeyword);

            renderGeneric(container, "#entry-template", "jobroles");
            return false;
        }
    });

    // Add job role
    $('#jobroles-description').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            var container = $('#jobrole-container');

            var title = readValueFromInput('#jobroles-title');
            var description = readValueFromInput('#jobroles-description');

            addData(container, { Title: title, Description: description });

            renderJobroles();
            return false;
        }
    });

    // competencyqualification
    $('#competencyqualification-input').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            var container = $("#competencyqualification-container");
            var input = readValueFromInput("#competencyqualification-input");

            addData(container, input );
            renderGeneric(container, "#entry-template", "CompetencyQualification");

            return false;
        }
    });

    $('#knowledgequalification-input').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            var container = $("#knowledgequalification-container");
            var input = readValueFromInput("#knowledgequalification-input");

            addData(container, input);
            renderGeneric(container, "#entry-template", "KnowledgeQualification");

            return false;
        }
    });

    $('#combinedqualification-input').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            var container = $("#combinedqualification-container");
            var input = readValueFromInput("#combinedqualification-input");

            addData(container, input);
            renderGeneric(container, "#entry-template", "CombinedQualification");

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

    var setUpCalendar = function() {
        var date = new Date();
        var year = 1900 + date.getYear();

        $('.datepicker.from').datepicker({
            dateFormat: 'yy-dd-mm',
            changeMonth: true,
            changeYear: true,
            yearRange: "1990:" + year
        });

        $('.datepicker.to').datepicker({
            dateFormat: 'yy-dd-mm',
            changeMonth: true,
            changeYear: true,
            yearRange: year + ":" + (year + 20)
        });
    };
    // Init
    renderGeneric("#keywords-container", "#entry-template", "keywords");
    renderGeneric("#jobroles-container", "#entry-template", "jobroles");
    renderJobroles();
    renderGeneric($("#competencyqualification-container"), "#entry-template", "CompetencyQualification");
    renderGeneric("#knowledgequalification-container", "#entry-template", "KnowledgeQualification");
    renderGeneric("#combinedqualification-container", "#entry-template", "CombinedQualification");

    setUpCalendar();

    var top = $('.edit').offset().top;
    $(window).scroll(function () {
        var fromTop = $("body").scrollTop();
        $('.edit-header').toggleClass('fixed', (fromTop > top));
    });

})(this);