var FatMetaData = FatMetaData || {};

(function (editFramework) {
    "use strict";

    var mapEntry = function (x, i, name) {
        return { entry: x, name: name, index: i };
    }

    var mapJobRoles = function (x, i, name) {
        return { title: x.Title, description: x.Description, index: i };
    }

    var deleteItem = function (container, render) {

        $(container).find('.delete').on('click', function (e) {
            var val = $(this).attr('data-value');
            var jsons = JSON.parse($(container).attr("data-json"));
            var selectedItem = jsons[val];
            var newValues = jsons.filter(function (x) { return x !== selectedItem });

            $(container).attr('data-json', JSON.stringify(newValues));
            render();
        });
    }

    var renderItem = function (mapToObj, templateQuery, c) {
        var container = $(c);
        var name = container.attr('data-name');
        var jsons = JSON.parse($(container).attr("data-json"));
        var source = $(templateQuery).html();
        var template = Handlebars.compile(source);

        var html = jsons.map(function (x, i) {
            var context = mapToObj(x, i, name);
            return template(context);
        });

        $(container).html(html);
        deleteItem(c, renderItem.bind(null, mapToObj, templateQuery, c));
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

    $('.entrybox input').keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            var container = $(this.closest('.entrybox')).find('.property-container');
            var input = readValueFromInput(this);

            addData(container, input);
            renderItem(mapEntry, "#entry-template", container);

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

            renderItem(mapJobRoles, "#jobrole-template", "#jobroles-property .property-container");
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
    

    editFramework.init = function () {

        renderItem(mapEntry, "#entry-template", "#keyword-property .property-container");

        renderItem(mapJobRoles, "#jobrole-template", "#jobroles-property .property-container");

        renderItem(mapEntry, "#entry-template", "#competencyqualification-property .property-container");
        renderItem(mapEntry, "#entry-template", "#knowledgequalification-property .property-container");
        renderItem(mapEntry, "#entry-template", "#combinedqualification-property .property-container");

        setUpCalendar();

        var top = $('.edit').offset().top;
        $(window).scroll(function () {
            var fromTop = $("body").scrollTop();
            $('.edit-header').toggleClass('fixed', (fromTop > top));
        });

    }

    //$('input').on('change', function() {
    //    $('.edit').addClass('changed');
    //});

    //$('input[type=submit]').on('click', function() {
    //    $('.edit').removeClass('changed');
    //});

    //$(window).bind('beforeunload', function () {
    //    if ($('.edit').hasClass('changed')) {
    //        return 'Are you sure you want to leave?';
    //    }
    //});

}(FatMetaData.EditFramework = {}));