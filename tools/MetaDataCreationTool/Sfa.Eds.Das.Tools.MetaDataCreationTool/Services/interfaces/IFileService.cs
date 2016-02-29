namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces
{
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Models;

    public interface IFileService
    {
        void CreateJsonFile(Standard standard, string path, bool overwrite);
        void UpdateJsonFile(Standard standard, string path);
    }
}