namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.Collections.Generic;

    public interface IGetStandardMetaData
    {
        /// <summary>
        /// Returns all standards
        /// </summary>
        /// <returns>List of jsonstings</returns>
        IEnumerable<string> GetAllAsJson();
    }
}