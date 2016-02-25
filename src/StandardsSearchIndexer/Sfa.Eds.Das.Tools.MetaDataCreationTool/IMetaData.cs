namespace Sfa.Eds.Das.Tools.MetaDataCreationTool
{
    using System.Collections.Generic;

    public interface IMetaData
    {
        /// <summary>
        ///
        ///    Will
        ///    - download zip file from course director and unzip standard.csv file.
        ///    - Creates metadata json for new standards and then push them to git repository
        /// </summary>
        void GenerateStandardMetadataFiles();

        /// <summary>
        /// Returns all standards
        /// </summary>
        /// <returns>List of jsonstings</returns>
        IEnumerable<string> GetStandards();
    }
}