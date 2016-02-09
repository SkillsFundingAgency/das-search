var StandardsMaster = kendo.Class.extend({

    /* --------------------------------- Construction ------------------------------------ */

    init: function (form, dataSource, itemsCountPerPage, webServiceUrl) {
        this._form = form;
        this._dataSource = dataSource;
        this._itemsCountPerPage = itemsCountPerPage;
        this._webServiceUrl = webServiceUrl;
        this._initialize();
    },

    /* --------------------------------- public methods ---------------------------------- */

    refreshPager: function (totalCount) {
        var that = this;
        
        /* a single page can hold the items so the pager is deleted */
        if (totalCount / this.get_itemsCountPerPage() <= 1) {
            $(this._masterElements.pagesWrapper).empty();
            this.set_pagesCount(1);
            return;
        }

        var pagesCount = totalCount % this.get_itemsCountPerPage() > 0 ?
            Math.floor(totalCount / this.get_itemsCountPerPage()) + 1 : Math.floor(totalCount / this.get_itemsCountPerPage());

        /* the pages count is not changed */
        if (pagesCount == this.get_pagesCount()) {
            return;
        }
        if ($(this._masterElements.pagesWrapper).children().length == 0 && pagesCount > 1) {
            /* a single page cannot hold all items so pager is added */
            $(this._masterElements.pagesWrapper).append('<a class="rgCurrentPage pageNumber"><span>' + 1 + '</span></a>');
            for (var i = 2; i <= pagesCount; i++) {
                $(this._masterElements.pagesWrapper).append('<a class="pageNumber"><span>' + i + '</span></a>');
            }
        }
        else if (pagesCount < this.get_pagesCount()) {
            /* remove the last page from the pager */
            $(this._masterElements.pagesWrapper).children().last().remove();

            if ($(this._masterElements.pagesWrapper).children(".rgCurrentPage").length == 0) {
                /* the selected page was removed so set the last page to be the current page */
                $(this._masterElements.pagesWrapper).children().last().addClass("rgCurrentPage");
                this.set_currentPage(pagesCount);
            }
        }
        else if (pagesCount > this.get_pagesCount()) {
            /* add new page to the pager */
            $(this._masterElements.pagesWrapper).append('<a class="pageNumber"><span>' + Math.floor(pagesCount) + '</span></a>');
        }

        this.set_pagesCount(pagesCount);

        $(this._masterElements.pagesWrapper).children(".pageNumber").click(function () {
            /* change the selected page */
            var selectedPage = this.firstChild.innerHTML;
            if (that.get_currentPage() == selectedPage) {
                return;
            }
            $(that._masterElements.pagesWrapper).children(".rgCurrentPage").removeClass("rgCurrentPage");
            this.className = this.className + " rgCurrentPage";

            /* load items for the new page */
            that.set_currentPage(selectedPage);
            that.set_skip((that.get_currentPage() - 1) * that.get_itemsCountPerPage());
            that.get_dataSource().read();
        });
    },
    
    /* --------------------------------- event handlers ---------------------------------- */

    _gridBound: function () {
        if (this.get_dataSource().data().length == 0) {
            $(this._masterElements.decisionScreen).show();
            $(this._masterElements.masterGrid).hide();
            $("#toolbar").hide();
            return;
        }
        $(this._masterElements.decisionScreen).hide();
        $("#toolbar").show();
        $(".sfActionsMenu").kendoMenu({ animation: false, openOnClick: true });
        $(this._masterElements.masterGrid).show();

        var that = this;
        $('input[type="checkbox"][data-command="check"]').click(function () {
            if ($(this).is(":checked")) {
                if ($('input[type="checkbox"][data-command="check"]:unchecked').length == 0) {
                    $(that._masterElements.checkAllCheckbox).attr('checked', true);
                }
                that._enableBatchActions(that);
            }
            else {
                $(that._masterElements.checkAllCheckbox).attr('checked', false);
                if ($('input[type="checkbox"][data-command="check"]:checked').length == 0) {
                    that._disableBatchActions(that);
                }
            }
        });
        $('a[data-command="delete"]').click(function () {
            $(that._masterElements.confirmDeleteButton).attr("data-id", $(this).attr("data-id"));
            that.get_deleteConfirmationWindow().open();
            that.get_deleteConfirmationWindow().center();
            jQuery(that.get_deleteConfirmationWindow().element).parent().css({ "top": that.get_dialogScrollTop() });
        });
        $('a[data-command="edit"]').click(function () {
            var id = $(this).attr("data-id");
            that.get_form().show(id);
        });
    },

    /* --------------------------------- private methods --------------------------------- */

    _initialize: function () {
        this._initializeDialogs();

        var that = this;

        /* handle the check all checkbox */
        $(this._masterElements.checkAllCheckbox).click(function () {
            var isChecked = $(this).is(':checked');
            $('input[type="checkbox"][data-command="check"]').attr('checked', isChecked);
            if (isChecked) {
                that._enableBatchActions(that);
            }
            else {
                that._disableBatchActions(that);
            }
        });

        this._initializeMasterGrid();

        this._initializeView();

        this._initializeSortingDropDownList();
    },

    _initializeDialogs: function () {
        var dialogSettings = {
            resizable: false,
            width: "425px",
            height: "auto",
            title: "Modal Window",
            modal: true,
            visible: false,
            animation: false,
            actions: ["Close"]
        }

        this._initializeDeleteDialog(dialogSettings);

        this._initializeBatchDeleteDialog(dialogSettings);

        this._initializeCustomSortingDialog(dialogSettings);
    },

    _initializeMasterGrid: function () {
        var gridBoundDelegate = $.proxy(this._gridBound, this);
        $(this._masterElements.masterGrid).kendoGrid({
            dataSource: this.get_dataSource(),
            rowTemplate: kendo.template($(this._masterElements.masterRowTemplate).html()),
            scrollable: false,
            dataBound: gridBoundDelegate
        });
    },

    _initializeView: function () {
        var actionName = this._getParameterByName("action");
        if (actionName == "editStandard") {
            var id = this._getParameterByName("standardId");
            that.get_form().show(id);
        }
        else if (actionName == "newStandard") {
            that.get_form().show();
        }
    },

    _initializeSortingDropDownList: function () {
        var that = this;

        var sortExpressions = [
            { text: "Last created on top", value: "DateCreated DESC" },
            { text: "Last modified on top", value: "LastModified DESC" },
            { text: "By Title (A-Z)", value: "Title ASC" },
            { text: "By Title (Z-A)", value: "Title DESC" },
            { text: "----------", value: "unselectable" },
            { text: "Custom sorting...", value: "custom" }
        ];

        $(this._masterElements.sortingDropDownList).kendoDropDownList({
            dataTextField: "text",
            dataValueField: "value",
            dataSource: sortExpressions,
            select: function (e) {
                that.set_previousSelectedSortingIndex(that.get_sortingDropDownList().selectedIndex);
                if (e.item.text() == "----------") {
                    e.preventDefault();
                    return false;
                }
            },
            change: function (e) {
                var expression;

                if (e.sender.dataItem().value == "custom") {
                    $(that._masterElements.customSortingStandardPropertiesDropDownList).kendoDropDownList({
                        dataTextField: "Name",
                        dataValueField: "Name",
                        dataSource: that.get_standardPropertiesDataSource()
                    });
                    that.set_customSortingStandardPropertiesDropDownList($(that._masterElements.customSortingStandardPropertiesDropDownList).data("kendoDropDownList"));
                    that.set_isCustomSortingChanged(false);

                    that.get_customSortingWindow().open();
                    that.get_customSortingWindow().center();
                    jQuery(that.get_customSortingWindow().element).parent().css({ "top": that.get_dialogScrollTop() });
                }
                else if (e.sender.dataItem().value != "unselectable") {
                    expression = e.sender.dataItem().value;
                    that.set_sortExpression(expression);
                    that.get_dataSource().read();
                }
            }
        });
        this.set_sortingDropDownList($(this._masterElements.sortingDropDownList).data("kendoDropDownList"));
    },

    _initializeDeleteDialog: function (dialogSettings) {
        var that = this;

        $(this._masterElements.deleteConfirmationDialog).kendoWindow(dialogSettings);
        this.set_deleteConfirmationWindow($(this._masterElements.deleteConfirmationDialog).data("kendoWindow"));
        $(this._masterElements.confirmDeleteButton).click(function () {
            var key = $(this).attr("data-id");
            $.ajax({
                type: 'DELETE',
                url: that.get_webServiceUrl() + key + "/",
                success: function () {
                    that._disableBatchActions(that);
                    that.get_dataSource().read();
                    that.get_deleteConfirmationWindow().close();
                }
            });
        });
        $(this._masterElements.cancelDeleteButton).click(function () {
            that.get_deleteConfirmationWindow().close();
        });
    },

    _initializeBatchDeleteDialog: function (dialogSettings) {
        var that = this;

        $(this._masterElements.batchDeleteConfirmationDialog).kendoWindow(dialogSettings);
        this.set_batchDeleteConfirmationWindow($(this._masterElements.batchDeleteConfirmationDialog).data("kendoWindow"));
        $(this._masterElements.confirmBatchDeleteButton).click(function () {
            var keys = new Array();
            $("input[type='checkbox'][data-command='check']:checked").each(function () {
                keys.push($(this).attr("data-id"));
            });
            $.ajax({
                type: 'POST',
                url: that.get_webServiceUrl() + "batch/",
                contentType: "application/json",
                processData: false,
                data: JSON.stringify(keys),
                success: function () {
                    that._disableBatchActions(that);
                    that.get_dataSource().read();
                    that.get_batchDeleteConfirmationWindow().close();
                }
            });
        });
        $(this._masterElements.cancelBatchDeleteButton).click(function () {
            that.get_batchDeleteConfirmationWindow().close();
        });
    },

    _initializeCustomSortingDialog: function (dialogSettings) {
        var that = this;

        var propertiesDataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: that.get_webServiceUrl() + "model/properties/",
                    dataType: "json",
					cache: false,
                }
            },
            schema: {
                model: {
                    id: "Name"
                },
                data: function (result) {
                    var items = result.Items;
                    return items;
                }
            }
        });
        this.set_standardPropertiesDataSource(propertiesDataSource);

        $(this._masterElements.customSortingDialog).kendoWindow(dialogSettings);
        this.set_customSortingWindow($(this._masterElements.customSortingDialog).data("kendoWindow"));
        this.get_customSortingWindow().bind("close", function (e) {
            if (!that.get_isCustomSortingChanged()) {
                that.get_sortingDropDownList().select(that.get_previousSelectedSortingIndex());
            }
            else {
                that.get_sortingDropDownList().select(that.get_sortingDropDownList().dataSource.data().length - 2);
            }
        });
        $(this._masterElements.saveCustomSortingButton).click(function () {
            var selectedProperty = that.get_customSortingStandardPropertiesDropDownList().value();
            var sortingFormat = $("#ascendingRadioButton").prop("checked") ? $("#ascendingRadioButton").val() : $("#descendingRadioButton").val();
            var expression = selectedProperty + " " + sortingFormat;
            that.set_sortExpression(expression);

            var itemsCount = that.get_sortingDropDownList().dataSource.data().length;

            if (that.get_sortingDropDownList().text() != "Edit custom sorting...") {
                /* add custom sorting for first time */
                that.get_sortingDropDownList().dataSource.insert(itemsCount - 1, { text: "Custom sorting", value: expression });
                that.get_sortingDropDownList().dataSource.data()[itemsCount].set("text", "Edit custom sorting...");
            }
            else {
                that.get_sortingDropDownList().dataSource.data()[itemsCount - 2].set("value", expression);
            }

            that.get_dataSource().read();
            that.set_isCustomSortingChanged(true);
            that.get_customSortingWindow().close();
        });
        $(this._masterElements.cancelCustomSortingButton).click(function () {
            that.get_customSortingWindow().close();
        });
    },

    _enableBatchActions: function (me) {
        /* enable the batch delete button */
        if ($(me._masterElements.batchDeleteButton).is('.sfDisabledLinkBtn')) {
            $(me._masterElements.batchDeleteButton).removeClass('sfDisabledLinkBtn');
            $(me._masterElements.batchDeleteButton).on('click', function () {
                var checkedTextBoxes = $("input[type='checkbox'][data-command='check']:checked");
                if (checkedTextBoxes.length == 1) {
                    $(me._masterElements.confirmDeleteButton).attr("data-id", $(checkedTextBoxes).attr("data-id"));
                    me.get_deleteConfirmationWindow().open();
                    me.get_deleteConfirmationWindow().center();
                    jQuery(me.get_deleteConfirmationWindow().element).parent().css({ "top": me.get_dialogScrollTop() });
                }
                else {
                    $(me._masterElements.batchDeleteStandardCountLabel).text(checkedTextBoxes.length);
                    me.get_batchDeleteConfirmationWindow().open();
                    me.get_batchDeleteConfirmationWindow().center();
                    jQuery(me.get_batchDeleteConfirmationWindow().element).parent().css({ "top": me.get_dialogScrollTop() });
                }
            });
        }
    },

    _disableBatchActions: function (me) {
        $(me._masterElements.checkAllCheckbox).attr('checked', false);
        $('input[type="checkbox"][data-command="check"]').attr('checked', false);

        /* disable the batch delete button */
        if ($(me._masterElements.batchDeleteButton).not('.sfDisabledLinkBtn')) {
            $(me._masterElements.batchDeleteButton).addClass('sfDisabledLinkBtn');
            $(me._masterElements.batchDeleteButton).off('click');
        }
    },
    
    /* gets the query string value be it's name */
    _getParameterByName: function (name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.search);
        if (results == null) {
            return "";
        }
        else {
            return decodeURIComponent(results[1].replace(/\+/g, " "));
        }
    },

    /* sets the datasource url */
    _setDataSourceActualUrl: function () {
        this.get_dataSource().transport.options.read.url = this.get_webServiceUrl() + "?"
            + (this.get_provider() ? "provider=" + this.get_provider() + "&" : "")
            + (this.get_sortExpression() ? "sortExpression=" + this.get_sortExpression() + "&" : "")
            + (this.get_skip() ? "skip=" + this.get_skip() + "&" : "")
            + (this.get_take() ? "take=" + this.get_take() + "&" : "")
            + (this.get_filter() ? "filter=" + this.get_filter() : "");
    },

    /* --------------------------------- properties -------------------------------------- */

    /* calculates top position of kendo dialog */
    get_dialogScrollTop: function () {
        var scrollTopHtml = jQuery("html").eq(0).scrollTop();
        var scrollTopBody = jQuery("body").eq(0).scrollTop();
        var scrollTop = ((scrollTopHtml > scrollTopBody) ? scrollTopHtml : scrollTopBody) + 50;
        return scrollTop;
    },

    get_form: function () {
        return this._form;
    },

    get_dataSource: function () {
        return this._dataSource;
    },

    get_itemsCountPerPage: function () {
        return this._itemsCountPerPage;
    },

    get_webServiceUrl: function () {
        return this._webServiceUrl;
    },

    get_deleteConfirmationWindow: function () {
        return this._deleteConfirmationWindow;
    },
    set_deleteConfirmationWindow: function (window) {
        this._deleteConfirmationWindow = window;
    },

    get_batchDeleteConfirmationWindow: function () {
        return this._batchDeleteConfirmationWindow;
    },
    set_batchDeleteConfirmationWindow: function (window) {
        this._batchDeleteConfirmationWindow = window;
    },

    get_customSortingWindow: function () {
        return this._customSortingWindow;
    },
    set_customSortingWindow: function (window) {
        this._customSortingWindow = window;
    },

    get_sortingDropDownList: function () {
        return this._sortingDropDownList;
    },
    set_sortingDropDownList: function (dropDownList) {
        this._sortingDropDownList = dropDownList;
    },

    get_standardPropertiesDataSource: function () {
        return this._standardPropertiesDataSource;
    },
    set_standardPropertiesDataSource: function (dataSource) {
        this._standardPropertiesDataSource = dataSource;
    },

    get_customSortingStandardPropertiesDropDownList: function () {
        return this._customSortingStandardPropertiesDropDownList;
    },
    set_customSortingStandardPropertiesDropDownList: function (dropDownList) {
        this._customSortingStandardPropertiesDropDownList = dropDownList;
    },

    get_previousSelectedSortingIndex: function () {
        return this._previousSelectedSortingIndex;
    },
    set_previousSelectedSortingIndex: function (index) {
        this._previousSelectedSortingIndex = index;
    },

    get_isCustomSortingChanged: function () {
        return this._isCustomSortingChanged;
    },
    set_isCustomSortingChanged: function (isChanged) {
        this._isCustomSortingChanged = isChanged;
    },

    get_currentPage: function () {
        return this._currentPage;
    },
    set_currentPage: function (page) {
        this._currentPage = page;
    },

    get_pagesCount: function () {
        return this._pagesCount;
    },
    set_pagesCount: function (pagesCount) {
        this._pagesCount = pagesCount;
    },

    get_provider: function () {
        return this._provider;
    },
    set_provider: function (provider) {
        this._provider = provider;
        this._setDataSourceActualUrl();
    },

    get_sortExpression: function () {
        return this._sortExpression;
    },
    set_sortExpression: function (expression) {
        this._sortExpression = expression;
        this._setDataSourceActualUrl();
    },

    get_skip: function () {
        return this._skip;
    },
    set_skip: function (skip) {
        this._skip = skip;
        this._setDataSourceActualUrl();
    },

    get_take: function () {
        return this.get_itemsCountPerPage();
    },

    get_filter: function () {
        return this._filter;
    },
    set_filter: function (filter) {
        this._filter = filter;
        this._setDataSourceActualUrl();
    },


    /* --------------------------------- private fields ---------------------------------- */

    _masterElements: {
        masterGrid: "#standardsGrid",
        pagesWrapper: "#pagesWrapper",
        masterRowTemplate: "#standardsRowTemplate",
        decisionScreen: "#standardsDecisionScreen",
        checkAllCheckbox: "#checkAllCheckbox",
        deleteConfirmationDialog: "#deleteStandardConfirmationDialog",
        confirmDeleteButton: "#confirmStandardDeleteButton",
        cancelDeleteButton: "#cancelDeleteStandardButton",
        batchDeleteButton: "#deleteUserStandards",
        batchDeleteConfirmationDialog: "#batchDeleteStandardConfirmationDialog",
        batchDeleteStandardCountLabel: "#batchDeleteStandardCountLabel",
        confirmBatchDeleteButton: "#confirmStandardBatchDeleteButton",
        cancelBatchDeleteButton: "#cancelBatchDeleteStandardButton",
        sortingDropDownList: "#sortingDropDownList",
        customSortingDialog: "#standardCustomSortingDialog",
        customSortingStandardPropertiesDropDownList: "#customSortingStandardPropertiesDropDownList",
        saveCustomSortingButton: "#saveCustomSortingButton",
        cancelCustomSortingButton: "#cancelCustomSortingButton"
    },
    _form: null,
    _dataSource: null,
    _webServiceUrl: null,
    _deleteConfirmationWindow: null,
    _batchDeleteConfirmationWindow: null,
    _customSortingWindow: null,
    _sortingDropDownList: null,
    _standardPropertiesDataSource: null,
    _customSortingStandardPropertiesDropDownList: null,
    _previousSelectedSortingIndex: null,
    _isCustomSortingChanged: null,
    _itemsCountPerPage: null,
    _currentPage: 1,
    _pagesCount: 1,
    _provider: null,
    _sortExpression: null,
    _skip: null,
    _filter: null
});