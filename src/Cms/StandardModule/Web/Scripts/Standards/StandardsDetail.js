var StandardsDetail = kendo.Class.extend({

    /* --------------------------------- Construction ------------------------------------ */

    init: function (form, dataSource, webServiceUrl) {
        form.kendoWindow({
            animation: false,
            modal: true,
            visible: false
        });
        form.parent().addClass("sfMaximizedWindow")
        this._formWindow = form.data("kendoWindow");
        this._dataSource = dataSource;
        this._webServiceUrl = webServiceUrl;
    },

    /* --------------------------------- public methods ---------------------------------- */

    show: function (id) {
        var that = this;
        this.reset();
        this.get_window().open();
        this.get_window().maximize();
        if (id) {
			$.ajax({
                type: 'GET',
                url: this.get_webServiceUrl() + id + "/",
                cache: false,
            }).done(function (data) {
                that.load(data.Item);
            });
            $("#createStandardButtonText").hide();
            $("#saveChangesStandardButtonText").show();
        }
        else {
            $("#createStandardButtonText").show();
            $("#saveChangesStandardButtonText").hide();
        }
    },

    close: function () {
        this.get_window().close();
    },

    load: function (data) {
        $(this._formElements.title).val(data.Title);
        $(this._formElements.description).val(data.Description);
        this.set_id(data.Id);
    },

    save: function () {
        if (this.isValid()) {
            var data = this._getFormData();
            var that = this;
            $.ajax({
                type: 'PUT',
                url: that.get_webServiceUrl() + that.get_id() + "/",
                contentType: "application/json",
                processData: false,
                data: JSON.stringify(data),
                success: function (result, args) {
                    that.close();
                    that.get_dataSource().read();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(Telerik.Sitefinity.JSON.parse(jqXHR.responseText).Detail);
                }
            });
        }
    },

    isValid: function () {
        var isValid = true;

        if ($(this._formElements.title).val().length == 0) {
            $(this._formElements.titleValidator).show();
            isValid = false;
        }
        else {
            $(this._formElements.titleValidator).hide();
        }
        if ($(this._formElements.title).val().length != 0 && $(this._formElements.title).val().length > 255) {
            $(this._formElements.titleLengthValidator).show();
            isValid = false;
        }
        else {
            $(this._formElements.titleLengthValidator).hide();
        }

        return isValid;
    },

    reset: function () {
        this.set_id("00000000-0000-0000-0000-000000000000");

        $(this._formElements.title).val("");
        $(this._formElements.titleValidator).hide();
        $(this._formElements.titleLengthValidator).hide();

        $(this._formElements.description).val("");
    },

    /* --------------------------------- event handlers ---------------------------------- */

    /* --------------------------------- private methods --------------------------------- */

    _getFormData: function () {
        var data = {
            "Item": {
                "Title": $(this._formElements.title).val(),
                "Description": $(this._formElements.description).val()
            }
        };
        return data;
    },

    /* --------------------------------- properties -------------------------------------- */

    get_window: function () {
        return this._formWindow;
    },

    get_dataSource: function () {
        return this._dataSource;
    },

    get_webServiceUrl: function () {
        return this._webServiceUrl;
    },

    get_id: function () {
        return this._id;
    },
    set_id: function (id) {
        this._id = id;
    },

    /* --------------------------------- private fields ---------------------------------- */

    _formElements: {
        title: "#standardTitle",
        titleValidator: "#standardTitleValidator",
        titleLengthValidator: "#standardTitleLengthValidator",
        description: "#standardDescription"
    },
    _formWindow: null,
    _dataSource: null,
    _webServiceUrl: null,
    _id: "00000000-0000-0000-0000-000000000000"
});