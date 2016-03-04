namespace Sfa.Eds.Das.Indexer.ApplicationServices.MetaData
{
    using System.Collections.Generic;

    public interface IGetStandardMetaData
    {
        /// <summary>
        /// Returns all standards
        /// </summary>
        /// <returns>List of jsonstings</returns>
        IDictionary<string, string> GetAllAsJson();
    }
}