using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Model;
using StandardModule.Models;

namespace StandardModule.Data.OpenAccess
{
    public class StandardModuleFluentMapping : OpenAccessFluentMappingBase
    {
        #region Construction
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardModuleFluentMapping" /> class.
        /// </summary>
        internal StandardModuleFluentMapping() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardModuleFluentMapping" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public StandardModuleFluentMapping(IDatabaseMappingContext context) : base(context)
        {
        }
        #endregion

        #region Public and overriden methods
        /// <summary>
        /// Gets the list of mapping configurations.
        /// </summary>
        /// <inheritdoc />
        /// <returns></returns>
        public override IList<MappingConfiguration> GetMapping()
        {
            var mappings = new List<MappingConfiguration>();

            this.MapStandards(mappings);

            return mappings;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Maps the Standard items.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        private void MapStandards(List<MappingConfiguration> mappings)
        {
            var mapping = new MappingConfiguration<Standard>();

            mapping.MapType(p => new { }).SetTableName("StandardModule_Standards", this.Context);

            mapping.HasProperty(p => p.Id).IsIdentity().IsNotNullable();
            mapping.HasProperty(p => p.Title).IsNotNullable().IsText(this.Context, 255);
            mapping.HasProperty(p => p.Description).IsLongText(this.Context);
            mapping.HasProperty(p => p.ApplicationName);
            mapping.HasProperty(p => p.LastModified);
            mapping.HasProperty(p => p.DateCreated);

            mappings.Add(mapping);
        }
        #endregion
    }
}