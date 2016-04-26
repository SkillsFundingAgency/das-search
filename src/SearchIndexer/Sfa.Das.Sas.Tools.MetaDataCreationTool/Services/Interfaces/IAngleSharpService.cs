using System.Collections.Generic;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces
{
    public interface IAngleSharpService
    {
        IList<string> GetLinks(string fromUrl, string selector, string textInTitle);
    }
}