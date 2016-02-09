<%@ Control Language="C#" %>

<fieldset class="sfNewItemForm">
    <a id="backToStandards" href="javascript:void(0);" class="sfBack sfCancelStandardButton">
        <asp:Literal ID="backToMasterLiteral" runat="server" Text='<%$Resources:StandardModuleResources, BackToLabel %>'></asp:Literal>
    </a>
    <h1>
        <asp:Literal ID="createAStandardLiteral" runat="server" Text='<%$Resources:StandardModuleResources, CreateAStandard %>'></asp:Literal>
    </h1>
    <div class="sfForm sfFirstForm">
        <ul class="sfFormIn">
            <li class="sfTitleField">
                <label for="standardTitle" class="sfTxtLbl">
                    <asp:Literal ID="standardTitleTitle" runat="server" Text='<%$Resources:StandardModuleResources, StandardTitleLabel %>'></asp:Literal>
                </label>
                <input id="standardTitle" type="text" class="sfTxt" />
                <div class="sfExample">
                    <asp:Literal ID="standardTitleDescription" runat="server" Text='<%$Resources:StandardModuleResources, StandardTitleDescription %>'></asp:Literal>
                </div>
                <div id="standardTitleValidator" class="sfError" style="display:none;">
                    <asp:Literal ID="standardTitleEmptyErrorLiteral" runat="server" Text='<%$Resources:StandardModuleResources, StandardTitleCannotBeEmpty %>'></asp:Literal>
                </div>
                <div id="standardTitleLengthValidator" class="sfError" style="display:none;">
                    <asp:Literal ID="standardTitleLengthErrorLiteral" runat="server" Text='<%$Resources:StandardModuleResources, StandardTitleInvalidLength %>'></asp:Literal>
                </div>
            </li>
            <li class="sfFormSeparator">
                <label for="standardDescription" class="sfTxtLbl">
                    <asp:Literal ID="standardDescriptionTitle" runat="server" Text='<%$Resources:StandardModuleResources, StandardDescriptionLabel %>'></asp:Literal>
                </label>
                <textarea id="standardDescription" rows="15" class="sfTxt"></textarea>
            </li>

        </ul>
    </div>
        
    <div class="sfButtonArea sfMainFormBtns">
        <a id="saveStandardButton" class="sfLinkBtn sfSave">
            <span id="createStandardButtonText" class="sfLinkBtnIn">
                <asp:Literal ID="createStandardButtonLiteral" runat="server" Text='<%$Resources:StandardModuleResources, CreateThisStandardButton %>' />
            </span>
            <span id="saveChangesStandardButtonText" class="sfLinkBtnIn" style="display:none;">
                <asp:Literal ID="saveChangesStandardButtonLiteral" runat="server" Text='<%$Resources:StandardModuleResources, SaveChangesLabel %>' />
            </span>
        </a>
        <a id="cancel" class="sfCancel sfCancelStandardButton">
            <asp:Literal ID="cancelLiteral1" runat="server" Text='<%$Resources:StandardModuleResources, CancelLabel %>' />
        </a>
    </div>
</fieldset>