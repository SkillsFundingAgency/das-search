using System;
using System.Linq;
using System.Reflection;
using Telerik.OpenAccess.Metadata;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Data.Linq;
using Telerik.Sitefinity.Model;
using StandardModule.Models;

namespace StandardModule.Data.OpenAccess
{
    public class StandardModuleOpenAccessDataProvider : StandardModuleDataProvider, IOpenAccessDataProvider
    {
        #region Properties
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public OpenAccessProviderContext Context { get; set; }
        #endregion

        #region Public and overriden methods
        /// <summary>
        /// Gets the meta data source.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The meta data source</returns>
        public MetadataSource GetMetaDataSource(IDatabaseMappingContext context)
        {
            return new StandardModuleStorageMetadataSource(context);
        }

        /// <summary>
        /// Gets a query of all the Standard items.
        /// </summary>
        /// <returns>The Standard items.</returns>
        public override IQueryable<Standard> GetStandards()
        {
            return SitefinityQuery
                .Get<Standard>(this, MethodBase.GetCurrentMethod())
                .Where(b => b.ApplicationName == this.ApplicationName);
        }

        /// <summary>
        /// Gets a Standard by a specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The Standard.</returns>
        public override Standard GetStandard(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be Empty Guid");

            return this.GetContext().GetItemById<Standard>(id.ToString());
        }

        /// <summary>
        /// Creates a new Standard and returns it.
        /// </summary>
        /// <returns>The new Standard.</returns>
        public override Standard CreateStandard()
        {
            Guid id = Guid.NewGuid();

            var item = new Standard(id, this.ApplicationName);

            if (id != Guid.Empty)
                this.GetContext().Add(item);

            return item;
        }

        /// <summary>
        /// Updates the Standard.
        /// </summary>
        /// <param name="entity">The Standard entity.</param>
        public override void UpdateStandard(Standard entity)
        {
            entity.LastModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Deletes the Standard.
        /// </summary>
        /// <param name="entity">The Standard entity.</param>
        public override void DeleteStandard(Standard entity)
        {
            this.GetContext().Remove(entity);
        }
        #endregion
    }
}