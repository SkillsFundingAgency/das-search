namespace Sfa.Das.Sas.ApplicationServices.Services.Interfaces
{
    using System.Collections.Generic;

    using Core.Models;

    public interface IMetaDataService
    {
        IEnumerable<StandardMetaData> GetStandards();

        StandardMetaData GetStandard(string id);

        IEnumerable<FrameworkMetaData> GetFrameworks();

        FrameworkMetaData GetFramework(string id);

        void UpdateFramework(FrameworkMetaData model);
        void UpdateStandard(StandardMetaData model);
    }
}