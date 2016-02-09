using System;
using System.Linq;
using StandardModule.Models;

namespace StandardModule.Web.Services.Standards.ViewModels
{
    /// <summary>
    /// Provides methods for translating view models to models and vice versa for the StandardModule module.
    /// </summary>
    public static class StandardsViewModelTranslator
    {
        #region Standard
        /// <summary>
        /// Translates Standard view model to Standard model.
        /// </summary>
        /// <param name="source">
        /// An instance of the <see cref="StandardViewModel"/>.
        /// </param>
        /// <param name="target">
        /// An instance of the <see cref="Standard"/>.
        /// </param>
        public static void ToModel(StandardViewModel source, Standard target, StandardModuleManager manager)
        {
            target.Title = source.Title;
            target.Description = source.Description;
        }

        /// <summary>
        /// Translates Standard to Standard view model.
        /// </summary>
        /// <param name="source">
        /// An instance of the <see cref="Standard"/>.
        /// </param>
        /// <param name="target">
        /// An instance of the <see cref="StandardViewModel"/>.
        /// </param>
        public static void ToViewModel(Standard source, StandardViewModel target, StandardModuleManager manager)
        {
            target.Id = source.Id;
            target.LastModified = source.LastModified;
            target.DateCreated = source.DateCreated;

            target.Title = source.Title;
            target.Description = source.Description;
        }
        #endregion
    }
}
