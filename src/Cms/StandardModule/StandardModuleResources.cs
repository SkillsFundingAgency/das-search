using System;
using System.Linq;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Localization.Data;

namespace StandardModule
{
    /// <summary>
    /// Localizable strings for the StandardModule module
    /// </summary>
    /// <remarks>
    /// You can use Sitefinity Thunder to edit this file.
    /// To do this, open the file's context menu and select Edit with Thunder.
    /// 
    /// If you wish to install this as a part of a custom module,
    /// add this to the module's Initialize method:
    /// App.WorkWith()
    ///     .Module(ModuleName)
    ///     .Initialize()
    ///         .Localization<StandardModuleResources>();
    /// </remarks>
    /// <see cref="http://www.sitefinity.com/documentation/documentationarticles/developers-guide/how-to/how-to-import-events-from-facebook/creating-the-resources-class"/>
    [ObjectInfo("StandardModuleResources", ResourceClassId = "StandardModuleResources", Title = "StandardModuleResourcesTitle", TitlePlural = "StandardModuleResourcesTitlePlural", Description = "StandardModuleResourcesDescription")]
    public class StandardModuleResources : Resource
    {
        #region Construction
        /// <summary>
        /// Initializes new instance of <see cref="StandardModuleResources"/> class with the default <see cref="ResourceDataProvider"/>.
        /// </summary>
        public StandardModuleResources()
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="StandardModuleResources"/> class with the provided <see cref="ResourceDataProvider"/>.
        /// </summary>
        /// <param name="dataProvider"><see cref="ResourceDataProvider"/></param>
        public StandardModuleResources(ResourceDataProvider dataProvider) : base(dataProvider)
        {
        }
        #endregion

        #region Class Description
        /// <summary>
        /// StandardModule Resources
        /// </summary>
        [ResourceEntry("StandardModuleResourcesTitle",
            Value = "StandardModule module labels",
            Description = "The title of this class.",
            LastModified = "2016/02/09")]
        public string StandardModuleResourcesTitle
        {
            get
            {
                return this["StandardModuleResourcesTitle"];
            }
        }

        /// <summary>
        /// StandardModule Resources Title plural
        /// </summary>
        [ResourceEntry("StandardModuleResourcesTitlePlural",
            Value = "StandardModule module labels",
            Description = "The title plural of this class.",
            LastModified = "2016/02/09")]
        public string StandardModuleResourcesTitlePlural
        {
            get
            {
                return this["StandardModuleResourcesTitlePlural"];
            }
        }

        /// <summary>
        /// Contains localizable resources for StandardModule module.
        /// </summary>
        [ResourceEntry("StandardModuleResourcesDescription",
            Value = "Contains localizable resources for StandardModule module.",
            Description = "The description of this class.",
            LastModified = "2016/02/09")]
        public string StandardModuleResourcesDescription
        {
            get
            {
                return this["StandardModuleResourcesDescription"];
            }
        }
        #endregion

        #region Labels
        /// <summary>
        /// word: Actions
        /// </summary>
        [ResourceEntry("ActionsLabel",
            Value = "Actions",
            Description = "word: Actions",
            LastModified = "2016/02/09")]
        public string ActionsLabel
        {
            get
            {
                return this["ActionsLabel"];
            }
        }

        /// <summary>
        /// Title of the link for closing the dialog and going back to the StandardModule module
        /// </summary>
        [ResourceEntry("BackToLabel",
            Value = "Go Back",
            Description = "Title of the link for closing the dialog and going back",
            LastModified = "2016/02/09")]
        public string BackToLabel
        {
            get
            {
                return this["BackToLabel"];
            }
        }

        /// <summary>
        /// word: Cancel
        /// </summary>
        [ResourceEntry("CancelLabel",
            Value = "Cancel",
            Description = "word: Cancel",
            LastModified = "2016/02/09")]
        public string CancelLabel
        {
            get
            {
                return this["CancelLabel"];
            }
        }

        /// <summary>
        /// word: Save
        /// </summary>
        /// <value>Save</value>
        [ResourceEntry("SaveLabel",
            Value = "Save",
            Description = "word: Save",
            LastModified = "2016/02/09")]
        public string SaveLabel
        {
            get
            {
                return this["SaveLabel"];
            }
        }

        /// <summary>
        /// phrase: Save changes
        /// </summary>
        [ResourceEntry("SaveChangesLabel",
            Value = "Save changes",
            Description = "phrase: Save changes",
            LastModified = "2016/02/09")]
        public string SaveChangesLabel
        {
            get
            {
                return this["SaveChangesLabel"];
            }
        }

        /// <summary>
        /// word: Delete
        /// </summary>
        [ResourceEntry("DeleteLabel",
            Value = "Delete",
            Description = "word: Delete",
            LastModified = "2016/02/09")]
        public string DeleteLabel
        {
            get
            {
                return this["DeleteLabel"];
            }
        }

        /// <summary>
        /// Phrase: Yes, delete these items
        /// </summary>
        /// <value>Yes, delete these items</value>
        [ResourceEntry("YesDeleteTheseItemsButton",
            Value = "Yes, delete these items",
            Description = "Phrase: Yes, delete these items",
            LastModified = "2016/02/09")]
        public string YesDeleteTheseItemsButton
        {
            get
            {
                return this["YesDeleteTheseItemsButton"];
            }
        }

        /// <summary>
        /// Text of the button that confirms deletion of an item.
        /// </summary>
        /// <value>Yes, delete this item</value>
        [ResourceEntry("YesDeleteThisItemButton",
            Value = "Yes, delete this item",
            Description = "Text of the button that confirms deletion of an item.",
            LastModified = "2016/02/09")]
        public string YesDeleteThisItemButton
        {
            get
            {
                return this["YesDeleteThisItemButton"];
            }
        }

        /// <summary>
        /// Phrase: items are about to be deleted. Continue?
        /// </summary>
        /// <value>items are about to be deleted. Continue?</value>
        [ResourceEntry("BatchDeleteConfirmationMessage",
            Value = "items are about to be deleted. Continue?",
            Description = "Phrase: items are about to be deleted. Continue?",
            LastModified = "2016/02/09")]
        public string BatchDeleteConfirmationMessage
        {
            get
            {
                return this["BatchDeleteConfirmationMessage"];
            }
        }

        /// <summary>
        /// word: Sort
        /// </summary>
        /// <value>Sort</value>
        [ResourceEntry("SortLabel",
            Value = "Sort",
            Description = "word: Sort",
            LastModified = "2016/02/09")]
        public string SortLabel
        {
            get
            {
                return this["SortLabel"];
            }
        }

        /// <summary>
        /// Phrase: Custom sorting
        /// </summary>
        /// <value>Custom sorting</value>
        [ResourceEntry("CustomSortingDialogHeader",
            Value = "Custom sorting",
            Description = "Phrase: Custom sorting",
            LastModified = "2016/02/09")]
        public string CustomSortingDialogHeader
        {
            get
            {
                return this["CustomSortingDialogHeader"];
            }
        }

        /// <summary>
        /// word: or
        /// </summary>
        /// <value>or</value>
        [ResourceEntry("OrLabel",
            Value = "or",
            Description = "word: or",
            LastModified = "2016/02/09")]
        public string OrLabel
        {
            get
            {
                return this["OrLabel"];
            }
        }

        /// <summary>
        /// Phrase: Sort by
        /// </summary>
        /// <value>Sort by</value>
        [ResourceEntry("SortByLabel",
            Value = "Sort by",
            Description = "Phrase: Sort by",
            LastModified = "2016/02/09")]
        public string SortByLabel
        {
            get
            {
                return this["SortByLabel"];
            }
        }

        /// <summary>
        /// word: Yes
        /// </summary>
        /// <value>Yes</value>
        [ResourceEntry("YesLabel",
            Value = "Yes",
            Description = "word: Yes",
            LastModified = "2013/06/26")]
        public string YesLabel
        {
            get
            {
                return this["YesLabel"];
            }
        }

        /// <summary>
        /// word: No
        /// </summary>
        /// <value>No</value>
        [ResourceEntry("NoLabel",
            Value = "No",
            Description = "word: No",
            LastModified = "2013/06/26")]
        public string NoLabel
        {
            get
            {
                return this["NoLabel"];
            }
        }
        #endregion

        #region Standards
        /// <summary>
        /// Messsage: PageTitle
        /// </summary>
        /// <value>Title for the Standard's page.</value>
        [ResourceEntry("StandardGroupPageTitle",
            Value = "Standard",
            Description = "The title of Standard's page.",
            LastModified = "2016/02/09")]
        public string StandardGroupPageTitle
        {
            get
            {
                return this["StandardGroupPageTitle"];
            }
        }

        /// <summary>
        /// Messsage: PageDescription
        /// </summary>
        /// <value>Description for the Standard's page.</value>
        [ResourceEntry("StandardGroupPageDescription",
            Value = "Standard",
            Description = "The description of Standard's page.",
            LastModified = "2016/02/09")]
        public string StandardGroupPageDescription
        {
            get
            {
                return this["StandardGroupPageDescription"];
            }
        }

        /// <summary>
		/// The URL name of Standard's page.
		/// </summary>
		[ResourceEntry("StandardGroupPageUrlName",
            Value = "StandardModule-Standard",
            Description = "The URL name of Standard's page.",
            LastModified = "2016/02/09")]
        public string StandardGroupPageUrlName
        {
            get
            {
                return this["StandardGroupPageUrlName"];
            }
        }

        /// <summary>
        /// Message displayed to user when no standards exist in the system
        /// </summary>
        /// <value>No standards have been created yet</value>
        [ResourceEntry("NoStandardsCreatedMessage",
            Value = "No standards have been created yet",
            Description = "Message displayed to user when no standards exist in the system",
            LastModified = "2016/02/09")]
        public string NoStandardsCreatedMessage
        {
            get
            {
                return this["NoStandardsCreatedMessage"];
            }
        }

        /// <summary>
        /// Title of the button for creating a new standard
        /// </summary>
        /// <value>Create a standard</value>
        [ResourceEntry("CreateAStandard",
            Value = "Create a standard",
            Description = "Title of the button for creating a new standard",
            LastModified = "2016/02/09")]
        public string CreateAStandard
        {
            get
            {
                return this["CreateAStandard"];
            }
        }

        /// <summary>
        /// Label for editing a new standard
        /// </summary>
        /// <value>Create a standard</value>
        [ResourceEntry("EditAStandard",
            Value = "Edit a standard",
            Description = "Label for editing a new standard",
            LastModified = "2016/02/09")]
        public string EditAStandard
        {
            get
            {
                return this["EditAStandard"];
            }
        }

        /// <summary>
        /// Standard Title.
        /// </summary>
        /// <value>Title</value>
        [ResourceEntry("StandardTitleLabel",
            Value = "Title",
            Description = "Standard Title.",
            LastModified = "2016/02/09")]
        public string StandardTitleLabel
        {
            get
            {
                return this["StandardTitleLabel"];
            }
        }

        /// <summary>
        /// Standard Title description.
        /// </summary>
        /// <value>Enter the item's title (required)</value>
        [ResourceEntry("StandardTitleDescription",
            Value = "Enter the item's title (required)",
            Description = "Standard Title description.",
            LastModified = "2016/02/09")]
        public string StandardTitleDescription
        {
            get
            {
                return this["StandardTitleDescription"];
            }
        }

        /// <summary>
        /// Error message displayed if the user does not enter standard Title.
        /// </summary>
        [ResourceEntry("StandardTitleCannotBeEmpty",
            Value = "The Title of the standard cannot be empty.",
            Description = "Error message displayed if the user does not enter standard Title.",
            LastModified = "2016/02/09")]
        public string StandardTitleCannotBeEmpty
        {
            get
            {
                return this["StandardTitleCannotBeEmpty"];
            }
        }

        /// <summary>
        /// Error message displayed if the user enters too long Title.
        /// </summary>
        /// <value>Title value is too long. Maximum allowed is 255 characters.</value>
        [ResourceEntry("StandardTitleInvalidLength",
            Value = "Title value is too long. Maximum allowed is 255 characters.",
            Description = "Error message displayed if the user enters too long Title.",
            LastModified = "2016/02/09")]
        public string StandardTitleInvalidLength
        {
            get
            {
                return this["StandardTitleInvalidLength"];
            }
        }

        /// <summary>
        /// Standard Description.
        /// </summary>
        /// <value>Description</value>
        [ResourceEntry("StandardDescriptionLabel",
            Value = "Description",
            Description = "Standard Description.",
            LastModified = "2016/02/09")]
        public string StandardDescriptionLabel
        {
            get
            {
                return this["StandardDescriptionLabel"];
            }
        }

        /// <summary>
        /// Message displayed to user when deleting a user standard.
        /// </summary>
        [ResourceEntry("DeleteStandardConfirmationMessage",
            Value = "Are you sure you want to delete this standard?",
            Description = "Message displayed to user when deleting a user standard.",
            LastModified = "2016/02/09")]
        public string DeleteStandardConfirmationMessage
        {
            get
            {
                return this["DeleteStandardConfirmationMessage"];
            }
        }

        /// <summary>
        /// phrase: Create this standard
        /// </summary>
        [ResourceEntry("CreateThisStandardButton",
            Value = "Create this standard",
            Description = "phrase: Create this standard",
            LastModified = "2016/02/09")]
        public string CreateThisStandardButton
        {
            get
            {
                return this["CreateThisStandardButton"];
            }
        }

        /// <summary>
        /// The URL name of Standard's page.
        /// </summary>
        /// <value>StandardMasterPageUrl</value>
        [ResourceEntry("StandardMasterPageUrl",
            Value = "StandardMasterPageUrl",
            Description = "The URL name of Standard's page.",
            LastModified = "2016/02/09")]
        public string StandardMasterPageUrl
        {
            get
            {
                return this["StandardMasterPageUrl"];
            }
        }
        #endregion
    }
}