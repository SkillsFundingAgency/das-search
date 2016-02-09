$(document).ready(function () {
    var webServiceUrl = $('#standardsServiceUrlHidden').val();
    
    var itemsCountPerPage = 50;
    var sortExpression = "DateCreated DESC";
    var itemsTotalCount;
    var isLastPageDeleted;

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: webServiceUrl + "?sortExpression=" + sortExpression + "&take=" + itemsCountPerPage,
                dataType: "json",
				cache: false,
            }
        },
        schema: {
            model: {
                id: "Id"
            },
            data: function (result) {
                itemsTotalCount = result.TotalCount;
                var items = result.Items;

                /* all items from the last page were deleted so the data source must be refreshed */
                isLastPageDeleted = (items.length == 0 && itemsTotalCount != 0);

                return items;
            }
        },
        change: function (e) {
            if (isLastPageDeleted) {
                /* refresh the data source */
                standardsMasterView.set_skip((standardsMasterView.get_currentPage() - 2) * standardsMasterView.get_itemsCountPerPage());
                standardsMasterView.get_dataSource().read();
                return;
            }
            standardsMasterView.refreshPager(itemsTotalCount);
        }
    });

    var standardsDetailView = new StandardsDetail($("#standardsDetailWindow"), dataSource, webServiceUrl);
    var standardsMasterView = new StandardsMaster(standardsDetailView, dataSource, itemsCountPerPage, webServiceUrl);
    standardsMasterView.set_sortExpression(sortExpression);

    jQuery("body").addClass("sfNoSidebar");

    $("#createUserStandard").click(function () {
        standardsDetailView.show();
    });

    $("#createStandardDecisionScreen").click(function () {
        standardsDetailView.show();
    });

    $(".sfCancelStandardButton").click(function () {
        standardsDetailView.close();
    });

    $("#saveStandardButton").click(function () {
        standardsDetailView.save();
    });
});