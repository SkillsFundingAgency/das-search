<%@ Control Language="C#" %>
<%@ Register TagPrefix="sitefinity" Assembly="Telerik.Sitefinity" Namespace="Telerik.Sitefinity.Web.UI" %>
<%@ Register TagPrefix="StandardModule" Assembly="StandardModule" Namespace="StandardModule.Web.UI.Standards" %>
<%@ Import Namespace="StandardModule" %>

<sitefinity:ResourceLinks ID="resourcesLinks" runat="server">
    <sitefinity:ResourceFile Name="Styles/Ajax.css" />
    <sitefinity:ResourceFile Name="Styles/DatePicker.css" />
    <sitefinity:ResourceFile Name="Styles/Grid.css" />
    <sitefinity:ResourceFile Name="Styles/ListView.css" />
    <sitefinity:ResourceFile Name="Styles/MaxWindow.css" />
    <sitefinity:ResourceFile Name="Styles/MenuMoreActions.css" />
    <sitefinity:ResourceFile Name="Styles/Tabstrip.css" />
    <sitefinity:ResourceFile Name="Styles/Window.css" />
    <sitefinity:ResourceFile Name="Telerik.Sitefinity.Resources.Scripts.JSON2.js" />
</sitefinity:ResourceLinks>
<sitefinity:ResourceLinks ID="resourcesLinks2" runat="server" UseEmbeddedThemes="true" Theme="Default">
    <sitefinity:ResourceFile Name="Telerik.Sitefinity.Resources.Scripts.Kendo.styles.kendo_common_min.css" Static="True" />
    <sitefinity:ResourceFile Name="Telerik.Sitefinity.Resources.Scripts.Kendo.styles.kendo_default_min.css" Static="True" />
    <sitefinity:ResourceFile Name="StandardModule.Web.Resources.CustomStylesKendoUIView.css" AssemblyInfo="StandardModule.StandardModuleClass, StandardModule" Static="True" />
</sitefinity:ResourceLinks>
<h1 class="sfBreadCrumb">
    <asp:Literal runat="server" Text='StandardModule'/>
</h1>
<div class="sfMain sfClearfix">
    <div class="sfContent">
        <!-- toolbar -->
        <div id="toolbar" class="sfAllToolsWrapper">
            <div class="sfAllTools">
                <ul class="sfActions">
                    <li class="sfMainAction">
                        <a id="createUserStandard" class="sfLinkBtn sfSave">
                            <span class="sfLinkBtnIn">
                                <asp:Literal ID="createAStandard" runat="server" Text='<%$Resources:StandardModuleResources, CreateAStandard %>'/>
                            </span>
                        </a>
                    </li>
                    <li class="sfGroupBtn">
                        <a id="deleteUserStandards" class="sfLinkBtn sfDisabledLinkBtn">
                            <span class="sfLinkBtnIn">
                                <asp:Literal ID="deleteUserStandardsLiteral" runat="server" Text='<%$Resources:StandardModuleResources, DeleteLabel %>'/>
                            </span>
                        </a>
                    </li>
                    <li class="sfQuickSort sfNoMasterViews sfDropdownList">
                        <asp:Literal ID="SortLiteral" runat="server" Text='<%$Resources:StandardModuleResources, SortLabel %>'/>
                        <input id="sortingDropDownList" />
                    </li>
                </ul>
            </div>
        </div>

        <!-- main area -->
        <div class="sfWorkArea" id="workArea">
            <div id="standardsMasterView">
                <StandardModule:StandardsMaster id="StandardsMaster" runat="server" />
            </div>
            <div id="standardsDetailWindow" class="sfDialog sfFormDialog k-sitefinity">
                <StandardModule:StandardsDetail id="StandardsDetail" runat="server" />
            </div>
        </div>
    </div>
</div>

<input id="standardsServiceUrlHidden" type="hidden" value="<%= VirtualPathUtility.ToAbsolute("~/" + StandardModuleClass.StandardsWebServiceUrl)  %>" />
