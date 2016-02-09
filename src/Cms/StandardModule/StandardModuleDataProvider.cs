using System;
using System.Linq;
using Telerik.Sitefinity.Data;
using StandardModule.Models;

namespace StandardModule
{
    public abstract class StandardModuleDataProvider : DataProviderBase
    {
        #region Public and overriden methods
        /// <summary>
        /// Gets the known types.
        /// </summary>
        public override Type[] GetKnownTypes()
        {
            if (knownTypes == null)
            {
                knownTypes = new Type[]
                {
                    typeof(Standard)
                };
            }
            return knownTypes;
        }

        /// <summary>
        /// Gets the root key.
        /// </summary>
        /// <value>The root key.</value>
        public override string RootKey
        {
            get
            {
                return "StandardModuleDataProvider";
            }
        }
        #endregion

        #region Abstract methods
        /// <summary>
        /// Creates a new Standard and returns it.
        /// </summary>
        /// <returns>The new Standard.</returns>
        public abstract Standard CreateStandard();

        /// <summary>
        /// Gets a Standard by a specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The Standard.</returns>
        public abstract Standard GetStandard(Guid id);

        /// <summary>
        /// Gets a query of all the Standard items.
        /// </summary>
        /// <returns>The Standard items.</returns>
        public abstract IQueryable<Standard> GetStandards();

        /// <summary>
        /// Updates the Standard.
        /// </summary>
        /// <param name="entity">The Standard entity.</param>
        public abstract void UpdateStandard(Standard entity);

        /// <summary>
        /// Deletes the Standard.
        /// </summary>
        /// <param name="entity">The Standard entity.</param>
        public abstract void DeleteStandard(Standard entity);
        #endregion

        #region Private fields and constants
        private static Type[] knownTypes;
        #endregion
    }
}