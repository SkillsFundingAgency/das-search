using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.GenericContent.Configuration;
using Telerik.Sitefinity.Web.Configuration;
using StandardModule.Data.OpenAccess;

namespace StandardModule.Configuration
{
    /// <summary>
    /// Represents the configuration section for StandardModule module.
    /// </summary>
    [ObjectInfo(Title = "StandardModule Config Title", Description = "StandardModule Config Description")]
    public class StandardModuleConfig : ModuleConfigBase
    {
        #region Public and overriden methods
        protected override void InitializeDefaultProviders(ConfigElementDictionary<string, DataProviderSettings> providers)
        {
            providers.Add(new DataProviderSettings(providers)
            {
                Name = "StandardModuleOpenAccessDataProvider",
                Title = "Default Products",
                Description = "A provider that stores products data in database using OpenAccess ORM.",
                ProviderType = typeof(StandardModuleOpenAccessDataProvider),
                Parameters = new NameValueCollection() { { "applicationName", "/StandardModule" } }
            });
        }

        /// <summary>
        /// Gets or sets the name of the default data provider. 
        /// </summary>
        [DescriptionResource(typeof(ConfigDescriptions), "DefaultProvider")]
        [ConfigurationProperty("defaultProvider", DefaultValue = "StandardModuleOpenAccessDataProvider")]
        public override string DefaultProvider
        {
            get
            {
                return (string)this["defaultProvider"];
            }
            set
            {
                this["defaultProvider"] = value;
            }
        }
        #endregion
    }
}