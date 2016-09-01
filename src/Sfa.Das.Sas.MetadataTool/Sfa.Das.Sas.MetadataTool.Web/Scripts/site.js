"use strict";

var FatMetaData = FatMetaData || {};

FatMetaData.base = function() {

    var $t = function () {
        return {
            exists: function (s) {
                return ($(s).length > 0) ? true : false;
            }
        };
    }();

    function shouldLoadModule(selector) {
        if ($t.exists(selector))
            return true;
        return false;
    }

    function loadModules() {
        if (shouldLoadModule('.edit')) {
            FatMetaData.EditFramework.init();
        }
    }

    return {
        init: function () {
            loadModules();
        }
    }
}();

$(document).ready(FatMetaData.base.init);