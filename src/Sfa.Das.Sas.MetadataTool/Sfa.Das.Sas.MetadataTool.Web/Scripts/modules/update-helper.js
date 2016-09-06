var FatMetaData = FatMetaData || {};

(function (self) {
    "use strict";

    self.models = {
        mapEntry: function(x, i, name) {
            return { entry: x, name: name, index: i };
        },

        mapJobRoles: function(x, i) {
            return { title: x.Title, description: x.Description, index: i };
        }
    };

    var deleteItem = function(container, render) {
        $(container).find('.delete').on('click', function() {
            var val = $(this).attr('data-value');
            var jsons = JSON.parse($(container).attr("data-json"));
            var selectedItem = jsons[val];
            var newValues = jsons.filter(function(x) { return x !== selectedItem });

            $(container).attr('data-json', JSON.stringify(newValues));
            render();
        });
    };

    self.renderItem = function(mapToObj, templateQuery, c) {
        var container = $(c);
        var name = container.attr('data-name');
        var jsons = JSON.parse($(container).attr("data-json"));
        var source = $(templateQuery).html();
        var template = Handlebars.compile(source);

        var html = jsons.map(function(x, i) {
            var context = mapToObj(x, i, name);
            return template(context);
        });

        $(container).html(html);
        deleteItem(c, self.renderItem.bind(null, mapToObj, templateQuery, c));
    };

    self.readValueFromInput = function(fieldId) {
        var val = $(fieldId)[0].value;
        $(fieldId)[0].value = "";
        return val;
    };

    var getJson = function(containersSelector) {
        var data = containersSelector.attr("data-json");
        var data2 = data === "" ? "[]" : data;
        return JSON.parse(data2);
    };

    self.addData = function(containerSelector, entry) {
        var jsons = getJson(containerSelector);
        jsons.push(entry);
        containerSelector.attr('data-json', JSON.stringify(jsons));
    };

    self.init = function() {
    };

}(FatMetaData.UpdateHelper = {}));