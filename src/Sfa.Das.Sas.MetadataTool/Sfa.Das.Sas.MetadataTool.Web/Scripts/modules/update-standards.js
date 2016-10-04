var FatMetaData = FatMetaData || {};

(function (editStandard) {

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
    };

    editStandard.init = function() {
        setUp();


        helper.renderItem(helper.models.mapEntry, "#entry-template", "#keyword-property .property-container");
        helper.renderItem(helper.models.mapEntry, "#entry-template", "#jobrole-property .property-container");

        var top = $('.edit').offset().top;
        $(window).scroll(function() {
            var fromTop = $("body").scrollTop();
            $('.edit-header').toggleClass('fixed', (fromTop > top));
        });
    };

}(FatMetaData.EditStandard = {}));