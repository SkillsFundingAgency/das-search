using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Web.UI.Kendo;

namespace StandardModule.Web.UI.Standards
{
    /// <summary>
    /// Container for all the user interface of the StandardModule module.
    /// </summary>
    public class StandardsPage : KendoView
    {
        #region Properties
        /// <summary>
        /// Obsolete. Use LayoutTemplatePath instead.
        /// </summary>
        protected override string LayoutTemplateName
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the layout template's relative or virtual path.
        /// </summary>
        public override string LayoutTemplatePath
        {
            get
            {
                if (string.IsNullOrEmpty(base.LayoutTemplatePath))
                    base.LayoutTemplatePath = StandardsPage.layoutTemplatePath;
                return base.LayoutTemplatePath;
            }
            set
            {
                base.LayoutTemplatePath = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag" /> value that
        /// corresponds to this Web server control. This property is used primarily by control
        /// developers.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Web.UI.HtmlTextWriterTag" /> enumeration values.</returns>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                //we use div wrapper tag to make easier common styling
                return HtmlTextWriterTag.Div;
            }
        }
        #endregion

        #region Control references
        #endregion

        #region Public and overriden methods
        /// <summary>
        /// Initializes the controls.
        /// </summary>
        /// <param name="container"></param>
        /// <remarks>
        /// Initialize your controls in this method. Do not override CreateChildControls method.
        /// </remarks>
        protected override void InitializeControls(GenericContainer container)
        {
        }

        /// <summary>
        /// Gets a collection of <see cref="T:System.Web.UI.ScriptReference" /> objects
        /// that define script resources that the control requires.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerable" /> collection of <see cref="T:System.Web.UI.ScriptReference" />
        /// objects.
        /// </returns>
        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            var scripts = new List<ScriptReference>(base.GetScriptReferences());
            var assemblyName = typeof(StandardsPage).Assembly.FullName;

            scripts.Add(new ScriptReference(StandardsPage.StandardsDetailScript, assemblyName));
            scripts.Add(new ScriptReference(StandardsPage.StandardsMasterScript, assemblyName));
            scripts.Add(new ScriptReference(StandardsPage.StandardsPageScript, assemblyName));

            return scripts;
        }
        #endregion

        #region Private fields and constants
        private static readonly string layoutTemplatePath = string.Concat(StandardModuleClass.ModuleVirtualPath, "StandardModule.Web.UI.Standards.StandardsPage.ascx");

        internal const string StandardsPageScript = "StandardModule.Web.Scripts.Standards.StandardsPage.js";
        internal const string StandardsMasterScript = "StandardModule.Web.Scripts.Standards.StandardsMaster.js";
        internal const string StandardsDetailScript = "StandardModule.Web.Scripts.Standards.StandardsDetail.js";
        #endregion
    }
}
