using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Telerik.Sitefinity.Data.Linq.Dynamic;
using Telerik.Sitefinity.Web.Services;
using StandardModule.Models;
using StandardModule.Web.Services.Standards.ViewModels;

namespace StandardModule.Web.Services.Standards
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class StandardsService : IStandardsService
    {
        #region IStandards
        /// <inheritdoc/>
        public CollectionContext<StandardViewModel> GetStandards(string provider, string sortExpression, int skip, int take, string filter)
        {
            ServiceUtility.DisableCache();
            return this.GetStandardsInternal(provider, sortExpression, skip, take, filter);
        }

        /// <inheritdoc/>
        public CollectionContext<StandardViewModel> GetStandardsInXml(string provider, string sortExpression, int skip, int take, string filter)
        {
            ServiceUtility.DisableCache();
            return this.GetStandardsInternal(provider, sortExpression, skip, take, filter);
        }

        /// <inheritdoc/>
        public ItemContext<StandardViewModel> SaveStandard(ItemContext<StandardViewModel> context, string standardId, string provider)
        {
            ServiceUtility.DisableCache();
            return this.SaveStandardInternal(context, standardId, provider);
        }

        /// <inheritdoc/>
        public ItemContext<StandardViewModel> SaveStandardInXml(ItemContext<StandardViewModel> context, string standardId, string provider)
        {
            ServiceUtility.DisableCache();
            return this.SaveStandardInternal(context, standardId, provider);
        }

        /// <inheritdoc/>
        public bool DeleteStandard(string standardId, string provider)
        {
            ServiceUtility.DisableCache();
            return this.DeleteStandardInternal(standardId, provider);
        }

        /// <inheritdoc/>
        public bool DeleteStandardInXml(string standardId, string provider)
        {
            ServiceUtility.DisableCache();
            return this.DeleteStandardInternal(standardId, provider);
        }

        /// <inheritdoc/>
        public bool BatchDeleteStandards(string[] ids, string provider)
        {
            ServiceUtility.DisableCache();
            return this.BatchDeleteStandardsInternal(ids, provider);
        }

        /// <inheritdoc/>
        public bool BatchDeleteStandardsInXml(string[] ids, string provider)
        {
            ServiceUtility.DisableCache();
            return this.BatchDeleteStandardsInternal(ids, provider);
        }

        /// <inheritdoc/>
        public ItemContext<StandardViewModel> GetStandard(string standardId)
        {
            ServiceUtility.DisableCache();
            return this.GetStandardInternal(standardId);
        }

        /// <inheritdoc/>
        public ItemContext<StandardViewModel> GetStandardInXml(string standardId)
        {
            ServiceUtility.DisableCache();
            return this.GetStandardInternal(standardId);
        }

        /// <inheritdoc/>
        public CollectionContext<StandardPropertyViewModel> GetProperties()
        {
            ServiceUtility.DisableCache();
            return this.GetPropertiesInternal();
        }
        #endregion

        #region Non-public methods
        /// <summary>
        /// Gets the standards internal.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        private CollectionContext<StandardViewModel> GetStandardsInternal(string provider, string sortExpression, int skip, int take, string filter)
        {
            var manager = StandardModuleManager.GetManager(provider);
            var standards = manager.GetStandards();

            var totalCount = standards.Count();

            if (!string.IsNullOrEmpty(sortExpression))
                standards = standards.OrderBy(sortExpression);

            if (!string.IsNullOrEmpty(filter))
                standards = standards.Where(filter);

            if (skip > 0)
                standards = standards.Skip(skip);

            if (take > 0)
                standards = standards.Take(take);

            var standardsList = new List<StandardViewModel>();

            foreach (var standard in standards)
            {
                var standardViewModel = new StandardViewModel();
                StandardsViewModelTranslator.ToViewModel(standard, standardViewModel, manager);
                standardsList.Add(standardViewModel);
            }

            return new CollectionContext<StandardViewModel>(standardsList) { TotalCount = totalCount };
        }

        /// <summary>
        /// Saves the standard internal.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="standardId">The standard id.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        private ItemContext<StandardViewModel> SaveStandardInternal(ItemContext<StandardViewModel> context, string standardId, string provider)
        {
            var manager = StandardModuleManager.GetManager(provider);
            var id = new Guid(standardId);

            Standard standard = null;

            if (id == Guid.Empty)
                standard = manager.CreateStandard();
            else
                standard = manager.GetStandard(id);

            StandardsViewModelTranslator.ToModel(context.Item, standard, manager);

            if (id != Guid.Empty)
                manager.UpdateStandard(standard);

            manager.SaveChanges();
            StandardsViewModelTranslator.ToViewModel(standard, context.Item, manager);
            return context;
        }

        /// <summary>
        /// Deletes the standard internal.
        /// </summary>
        /// <param name="standardId">The standard id.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        private bool DeleteStandardInternal(string standardId, string provider)
        {
            var manager = StandardModuleManager.GetManager(provider);
            manager.DeleteStandard(manager.GetStandard(new Guid(standardId)));
            manager.SaveChanges();

            return true;
        }

        /// <summary>
        /// Batches the delete standards internal.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        private bool BatchDeleteStandardsInternal(string[] ids, string provider)
        {
            var manager = StandardModuleManager.GetManager(provider);
            foreach (var stringId in ids)
            {
                var standardId = new Guid(stringId);
                manager.DeleteStandard(manager.GetStandard(standardId));
            }
            manager.SaveChanges();

            return true;
        }

        /// <summary>
        /// Gets the standard internal.
        /// </summary>
        /// <param name="standardId">The standard id.</param>
        /// <returns></returns>
        private ItemContext<StandardViewModel> GetStandardInternal(string standardId)
        {
            var standardIdGuid = new Guid(standardId);
            var manager = StandardModuleManager.GetManager();

            var standard = manager.GetStandard(standardIdGuid);
            var standardViewModel = new StandardViewModel();
            StandardsViewModelTranslator.ToViewModel(standard, standardViewModel, manager);

            return new ItemContext<StandardViewModel>()
            {
                Item = standardViewModel
            };
        }

        /// <summary>
        /// Gets the properties internal.
        /// </summary>
        /// <returns></returns>
        private CollectionContext<StandardPropertyViewModel> GetPropertiesInternal()
        {
            List<StandardPropertyViewModel> properties = new List<StandardPropertyViewModel>();
            foreach (var property in typeof(StandardModule.Models.Standard).GetProperties())
            {
                if (!this.systemProperties.Contains(property.Name))
                {
                    properties.Add(new StandardPropertyViewModel() { Name = property.Name });
                }
            }
            return new CollectionContext<StandardPropertyViewModel>(properties) { TotalCount = properties.Count() };
        }
        #endregion

        #region Non-public Fields
        private readonly IEnumerable<string> systemProperties = new List<string>()
        {
            "Id", "Transaction", "ApplicationName", "Provider",
        };
        #endregion
    }
}