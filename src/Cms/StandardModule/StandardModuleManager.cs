using System;
using System.Linq;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using StandardModule.Configuration;
using StandardModule.Models;

namespace StandardModule
{
    public class StandardModuleManager : ManagerBase<StandardModuleDataProvider>
    {
        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardModuleManager" /> class.
        /// </summary>
        public StandardModuleManager() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardModuleManager" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        public StandardModuleManager(string providerName) : base(providerName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardModuleManager" /> class.
        /// </summary>
        /// <param name="providerName">Name of the provider.</param>
        /// <param name="transactionName">Name of the transaction.</param>
        public StandardModuleManager(string providerName, string transactionName) : base(providerName, transactionName)
        {
        }
        #endregion

        #region Public and overriden methods
        /// <summary>
        /// Gets the default provider delegate.
        /// </summary>
        /// <value>The default provider delegate.</value>
        protected override GetDefaultProvider DefaultProviderDelegate
        {
            get
            {
                return () => Config.Get<StandardModuleConfig>().DefaultProvider;
            }
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public override string ModuleName
        {
            get
            {
                return StandardModuleClass.ModuleName;
            }
        }

        /// <summary>
        /// Gets the providers settings.
        /// </summary>
        /// <value>The providers settings.</value>
        protected override ConfigElementDictionary<string, DataProviderSettings> ProvidersSettings
        {
            get
            {
                return Config.Get<StandardModuleConfig>().Providers;
            }
        }

        /// <summary>
        /// Get an instance of the StandardModule manager using the default provider.
        /// </summary>
        /// <returns>Instance of the StandardModule manager</returns>
        public static StandardModuleManager GetManager()
        {
            return ManagerBase<StandardModuleDataProvider>.GetManager<StandardModuleManager>();
        }

        /// <summary>
        /// Get an instance of the StandardModule manager by explicitly specifying the required provider to use
        /// </summary>
        /// <param name="providerName">Name of the provider to use, or null/empty string to use the default provider.</param>
        /// <returns>Instance of the StandardModule manager</returns>
        public static StandardModuleManager GetManager(string providerName)
        {
            return ManagerBase<StandardModuleDataProvider>.GetManager<StandardModuleManager>(providerName);
        }

        /// <summary>
        /// Get an instance of the StandardModule manager by explicitly specifying the required provider to use
        /// </summary>
        /// <param name="providerName">Name of the provider to use, or null/empty string to use the default provider.</param>
        /// <param name="transactionName">Name of the transaction.</param>
        /// <returns>Instance of the StandardModule manager</returns>
        public static StandardModuleManager GetManager(string providerName, string transactionName)
        {
            return ManagerBase<StandardModuleDataProvider>.GetManager<StandardModuleManager>(providerName, transactionName);
        }

        /// <summary>
        /// Creates a Standard.
        /// </summary>
        /// <returns>The created Standard.</returns>
        public Standard CreateStandard()
        {
            return this.Provider.CreateStandard();
        }

        /// <summary>
        /// Updates the Standard.
        /// </summary>
        /// <param name="entity">The Standard entity.</param>
        public void UpdateStandard(Standard entity)
        {
            this.Provider.UpdateStandard(entity);
        }

        /// <summary>
        /// Deletes the Standard.
        /// </summary>
        /// <param name="entity">The Standard entity.</param>
        public void DeleteStandard(Standard entity)
        {
            this.Provider.DeleteStandard(entity);
        }

        /// <summary>
        /// Gets the Standard by a specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The Standard.</returns>
        public Standard GetStandard(Guid id)
        {
            return this.Provider.GetStandard(id);
        }

        /// <summary>
        /// Gets a query of all the Standard items.
        /// </summary>
        /// <returns>The Standard items.</returns>
        public IQueryable<Standard> GetStandards()
        {
            return this.Provider.GetStandards();
        }
        #endregion
    }
}