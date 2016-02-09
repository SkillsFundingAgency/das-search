using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using Telerik.Sitefinity.Utilities.MS.ServiceModel.Web;
using Telerik.Sitefinity.Web.Services;
using StandardModule.Web.Services.Standards.ViewModels;

namespace StandardModule.Web.Services.Standards
{
    /// <summary>
    /// Provides contracts for loading and manipulating StandardModule data items (e.g. standards)
    /// </summary>
    [ServiceContract]
    public interface IStandardsService
    {
        /// <summary>
        /// Gets all standards for the given provider. The results are returned in JSON format.
        /// </summary>
        /// <param name="provider">Name of the provider from which the standards ought to be retrieved.</param>
        /// <param name="sortExpression">Sort expression used to order the standards.</param>
        /// <param name="skip">Number of standards to skip in result set. (used for paging)</param>
        /// <param name="take">Number of standards to take in the result set. (used for paging)</param>
        /// <param name="filter">Dynamic LINQ expression used to filter the wanted result set.</param>
        /// <returns>
        /// Collection context object of <see cref="StandardViewModel"/> objects.
        /// </returns>
        [WebHelp(Comment = "Gets all standards of the StandardModule module for the given provider. The results are returned in JSON format.")]
        [WebGet(UriTemplate = "/?provider={provider}&sortExpression={sortExpression}&skip={skip}&take={take}&filter={filter}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        CollectionContext<StandardViewModel> GetStandards(string provider, string sortExpression, int skip, int take, string filter);

        /// <summary>
        /// Gets all standards for the given provider. The results are returned in XML format.
        /// </summary>
        /// <param name="provider">Name of the provider from which the standards ought to be retrieved.</param>
        /// <param name="sortExpression">Sort expression used to order the standards.</param>
        /// <param name="skip">Number of standards to skip in result set. (used for paging)</param>
        /// <param name="take">Number of standards to take in the result set. (used for paging)</param>
        /// <param name="filter">Dynamic LINQ expression used to filter the wanted result set.</param>
        /// <returns>
        /// Collection context object of <see cref="StandardViewModel"/> objects.
        /// </returns>
        [WebHelp(Comment = "Gets all standards of the StandardModule module for the given provider. The results are returned in XML format.")]
        [WebGet(UriTemplate = "/xml/?provider={provider}&sortExpression={sortExpression}&skip={skip}&take={take}&filter={filter}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        CollectionContext<StandardViewModel> GetStandardsInXml(string provider, string sortExpression, int skip, int take, string filter);

        /// <summary>
        /// Gets the standard by it's id. The results are returned in JSON format.
        /// </summary>
        /// <param name="standardId">Id of the standard to be retrieved.</param>
        /// <returns>
        /// Item context object of <see cref="StandardViewModel"/> objects.
        /// </returns>
        [WebHelp(Comment = "Gets the standard by it's id. The results are returned in JSON format.")]
        [WebGet(UriTemplate = "/{standardId}/", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ItemContext<StandardViewModel> GetStandard(string standardId);

        /// <summary>
        /// Gets the standard by it's id. The results are returned in JSON format.
        /// </summary>
        /// <param name="standardId">Id of the standard to be retrieved.</param>
        /// <returns>
        /// Item context object of <see cref="StandardViewModel"/> objects.
        /// </returns>
        [WebHelp(Comment = "Gets the standard by it's id. The results are returned in JSON format.")]
        [WebGet(UriTemplate = "/xml/{standardId}/", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ItemContext<StandardViewModel> GetStandardInXml(string standardId);

        /// <summary>
        /// Saves a standard. If the standard with specified id exists that standard will be updated; otherwise new standard will be created.
        /// The saved standard is returned in JSON format.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="standardId">The standard id.</param>
        /// <param name="provider">The provider name through which the standard ought to be saved.</param>
        /// <returns>The saved standard.</returns>
        [WebHelp(Comment = "Saves a standard. If the standard with specified id exists that standard will be updated; otherwise new standard will be created. The saved standard is returned in JSON format.")]
        [WebInvoke(Method = "PUT", UriTemplate = "/{standardId}/?provider={provider}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ItemContext<StandardViewModel> SaveStandard(ItemContext<StandardViewModel> context, string standardId, string provider);

        /// <summary>
        /// Saves a standard. If the standard with specified id exists that standard will be updated; otherwise new standard will be created.
        /// The saved standard is returned in XML format.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="standardId">The standard id.</param>
        /// <param name="provider">The provider name through which the standard ought to be saved.</param>
        /// <returns>The saved standard.</returns>
        [WebHelp(Comment = "Saves a standard. If the standard with specified id exists that standard will be updated; otherwise new standard will be created. The saved standard is returned in XML format.")]
        [WebInvoke(Method = "PUT", UriTemplate = "/xml/{standardId}/?provider={provider}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        ItemContext<StandardViewModel> SaveStandardInXml(ItemContext<StandardViewModel> context, string standardId, string provider);

        /// <summary>
        /// Deletes the standard.
        /// </summary>
        /// <param name="standardId">The standard id.</param>
        /// <param name="provider">The provider.</param>
        [WebHelp(Comment = "Deletes the standard.")]
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/{standardId}/?provider={provider}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteStandard(string standardId, string provider);

        /// <summary>
        /// Deletes the standard.
        /// </summary>
        /// <param name="standardId">The standard id.</param>
        /// <param name="provider">The provider.</param>
        [WebHelp(Comment = "Deletes the standard.")]
        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/xml/{standardId}/?provider={provider}", ResponseFormat = WebMessageFormat.Xml)]
        bool DeleteStandardInXml(string standardId, string provider);

        /// <summary>
        /// Deletes a collection of standards. Result is returned in JSON format.
        /// </summary>
        /// <param name="ids">An array of the ids of the standards to delete.</param>
        /// <param name="provider">The name of the standards provider.</param>
        /// <returns>True if all standards have been deleted; otherwise false.</returns>
        [WebHelp(Comment = "Deletes multiple standards.")]
        [WebInvoke(Method = "POST", UriTemplate = "/batch/?provider={provider}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        bool BatchDeleteStandards(string[] ids, string provider);

        /// <summary>
        /// Deletes a collection of standards. Result is returned in XML format.
        /// </summary>
        /// <param name="ids">An array of the ids of the standards to delete.</param>
        /// <param name="provider">The name of the standards provider.</param>
        /// <returns>True if all standards have been deleted; otherwise false.</returns>
        [WebHelp(Comment = "Deletes multiple standards.")]
        [WebInvoke(Method = "POST", UriTemplate = "/xml/batch/?provider={provider}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        bool BatchDeleteStandardsInXml(string[] ids, string provider);

        /// <summary>
        /// Gets the properties for the model. The results are returned in JSON format.
        /// </summary>
        /// <returns>
        /// Collection context object of <see cref="StandardPropertyViewModel"/> objects.
        /// </returns>
        [WebHelp(Comment = "Get standard properties.")]
        [WebGet(UriTemplate = "/model/properties/", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        CollectionContext<StandardPropertyViewModel> GetProperties();

    }
}