<%@ Control Language="C#" %>

<style type="text/css">
.k-loading-mask { visibility: hidden; }
</style>
<!-- no standards created screen -->
<div id="standardsDecisionScreen" style="display:none;" class="sfEmptyList">
    <p class="sfMessage sfMsgNeutral sfMsgVisible"><asp:Literal ID="noStandardsCreatedLiteral" runat="server" Text='<%$Resources:StandardModuleResources, NoStandardsCreatedMessage %>'></asp:Literal></p>
    <ol class="sfWhatsNext">
        <li class="sfCreateItem">
            <a id="createStandardDecisionScreen">
                <asp:Literal ID="createAStandardLiteral" runat="server" Text='<%$Resources:StandardModuleResources, CreateAStandard %>'></asp:Literal>
                <span class="sfDecisionIcon"></span>
            </a>
        </li>
    </ol>
</div>

<!-- standards grid -->
<table id="standardsGrid" class="rgTopOffset rgMasterTable" style="display: none;">
    <thead>
        <tr>
            <th class="sfCheckBoxCol">
                <input type="checkbox" id="checkAllCheckbox" name="checkAllCheckbox" value="" />
            </th>
            <th class="sfTitleCol">
                <asp:Literal ID="standardHeader" runat="server" Text='<%$Resources:StandardModuleResources, StandardTitleLabel %>'></asp:Literal>
            </th>
            <th class="sfLarge">
                <asp:Literal ID="descriptionHeader" runat="server" Text='<%$Resources:StandardModuleResources, StandardDescriptionLabel %>'></asp:Literal>
            </th>
            <th class="sfMoreActions sfLast">
                <asp:Literal ID="actionsHeader" runat="server" Text='<%$Resources:StandardModuleResources, ActionsLabel %>'></asp:Literal>
            </th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td colspan="4">
            </td>
        </tr>
    </tbody>
    <tfoot>
        <tr class="rgPager">
            <td class="rgPagerCell NumericPages" colspan="4">
                <div class="rgWrap rgNumPart">
                    <div id="pagesWrapper">
                    </div>
                </div>
            </td>
        </tr>
    </tfoot>
</table>
<!-- standards grid row template -->
<script id="standardsRowTemplate" type="text/x-kendo-template" style="display: none;">
    <tr>
        <td class="sfCheckBoxCol">
            <input type="checkbox" data-command="check" data-id="#= Id #" value=""/>
        </td>
        <td class="sfTitleCol">
            <a class="sfEditButton sfItemTitle sfactive" data-command="edit" data-id="#= Id #">
                <strong>#= Title #</strong>                
            </a>
        </td>
        <td class="sfLarge">
            <div class="dmDescription">#= Description #</div>
        </td>
        <td class="sfMoreActions sfLast">
            <ul class="sfActionsMenu">
                <li class="sfFirst sfLast">
                    #= ActionsLabel #
                    <ul>
                        <li>
                            <a class="sfDeleteItm" data-command="delete" data-id="#= Id #">
                                #= DeleteLabel #
                            </a>
                        </li>   
                    </ul>
                <li>
            </ul>
        </td>
    </tr>
</script>
<!-- END standards grid row template -->

<div id="deleteStandardConfirmationDialog" class="sfSelectorDialog">
    <p>
        <asp:Literal ID="deleteStandardConfirmationLiteral" runat="server" Text='<%$Resources:StandardModuleResources, DeleteStandardConfirmationMessage %>'/>
    </p>
    <div class="sfButtonArea">
        <a id="confirmStandardDeleteButton" class="sfLinkBtn sfDelete">
            <span class="sfLinkBtnIn">
                <asp:Literal ID="deleteStandardButtonLiteral" runat="server" Text='<%$Resources:StandardModuleResources, YesDeleteThisItemButton %>'/>
            </span>
        </a>
        <a id="cancelDeleteStandardButton" class="sfCancel">
            <asp:Literal ID="cancelDeleteLiteral" runat="server" Text='<%$Resources:StandardModuleResources, CancelLabel %>'/>
        </a>
    </div>
</div>

<div id="batchDeleteStandardConfirmationDialog" class="sfSelectorDialog">
    <p>
        <span id="batchDeleteStandardCountLabel"></span>
        <span id="batchDeleteStandardConfirmationSpan">
            <asp:Literal ID="batchDeleteStandardConfirmationLiteral" runat="server" Text='<%$Resources:StandardModuleResources, BatchDeleteConfirmationMessage %>'/>
        </span>
    </p>
    <div class="sfButtonArea">
        <a id="confirmStandardBatchDeleteButton" class="sfLinkBtn sfDelete">
            <span class="sfLinkBtnIn">
                <asp:Literal ID="batchDeleteStandardButtonLiteral" runat="server" Text='<%$Resources:StandardModuleResources, YesDeleteTheseItemsButton %>'/>
            </span>
        </a>
        <a id="cancelBatchDeleteStandardButton" class="sfCancel">
            <asp:Literal ID="cancelBatchDeleteLiteral" runat="server" Text='<%$Resources:StandardModuleResources, CancelLabel %>'/>
        </a>
    </div>
</div>

<div id="standardCustomSortingDialog" class="sfSelectorDialog">
    <h1><asp:literal ID="customSortingDialogHeader" runat="server" Text="<%$Resources:StandardModuleResources, CustomSortingDialogHeader%>" /></h1>
    <div class="sfSortingCondition">
        <div class="sfSortRules">
            <label class="sfTxtLbl" for="customSortingStandardPropertiesDropDownList">
                <asp:Literal ID="sortByLiteral" runat="server" Text="<%$Resources:StandardModuleResources, SortByLabel%>" />
            </label>
			<div class="sfDropdownList sfInlineBlock sfAlignTop">
				<input id="customSortingStandardPropertiesDropDownList" class="valid" />
			</div>
            <div class="sfInlineBlock">
                <ol class="sfRadioList">
                    <li>
                        <input id="ascendingRadioButton" type="radio" value="ASC" name="customSortingOrder" checked="checked">
                        <label for="ascendingRadioButton">Ascending</label>
                    </li>
                    <li>
                        <input id="descendingRadioButton" type="radio" value="DESC" name="customSortingOrder">
                        <label for="descendingRadioButton">Descending</label>
                    </li>
                </ol>
            </div>
        </div>
    </div>

    <div class="sfButtonArea sfSelectorBtns">
        <a ID="saveCustomSortingButton" Class="sfLinkBtn sfSave">
            <span class="sfLinkBtnIn">
                <asp:Literal ID="saveCustomSortingLiteral" runat="server" Text="<%$Resources:StandardModuleResources, SaveLabel %>" />
            </span>
        </a>
        <asp:Literal ID="orLiteral" runat="server" Text="<%$Resources:StandardModuleResources, OrLabel%>" />
        <a ID="cancelCustomSortingButton" Class="sfCancel">
            <asp:Literal ID="cancelCustomSortingLiteral" runat="server" Text="<%$Resources:StandardModuleResources, CancelLabel %>" />
        </a>
    </div>
</div>