var FatMetaData = FatMetaData || {};

(function (editFramework) {
    "use strict";

    var helper = FatMetaData.UpdateHelper;

    var setUp = function() {
        $('.entrybox input').keypress(function(e) {
            if (e.which === 13) {
                e.preventDefault();
                var container = $($(this).closest('.entrybox')).find('.property-container');
                var input = helper.readValueFromInput(this);

                helper.addData(container, input);
                helper.renderItem(helper.models.mapEntry, "#entry-template", container);

                return false;
            }
        });

        $('#jobroles-description').keypress(function(e) {
            if (e.which === 13) {
                e.preventDefault();
                var container = $('#jobrole-container');

                var title = helper.readValueFromInput('#jobroles-title');
                var description = helper.readValueFromInput('#jobroles-description');

                helper.addData(container, { Title: title, Description: description });

                helper.renderItem(helper.models.mapJobRoles, "#jobrole-template", "#jobroles-property .property-container");
                return false;
            }
        });

        $("#jobrole-template .edit").on('click', function () {
            var val = $(this).attr('data-value');
            var jsons = JSON.parse($(container).attr("data-json"));
            var selectedItem = jsons[val];
            var newValues = jsons.filter(function (x) { return x !== selectedItem });

            $(container).attr('data-json', JSON.stringify(newValues));
            render();
        });

        $('#jobroles-title').keypress(function(e) {
            if (e.which === 13) {
                e.preventDefault();
                $("#jobroles-description").focus();
                return false;
            }
            return true;
        });
    };

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

    editFramework.init = function() {

        setUp();

        helper.renderItem(helper.models.mapEntry, "#entry-template", "#keyword-property .property-container");
        helper.renderItem(helper.models.mapJobRoles, "#jobrole-template", "#jobroles-property .property-container");
        helper.renderItem(helper.models.mapEntry, "#entry-template", "#competencyqualification-property .property-container");
        helper.renderItem(helper.models.mapEntry, "#entry-template", "#knowledgequalification-property .property-container");
        helper.renderItem(helper.models.mapEntry, "#entry-template", "#combinedqualification-property .property-container");

        setUpCalendar();

        var top = $('.edit').offset().top;
        $(window).scroll(function() {
            var fromTop = $("body").scrollTop();
            $('.edit-header').toggleClass('fixed', (fromTop > top));
        });
    };

}(FatMetaData.EditFramework = {}));