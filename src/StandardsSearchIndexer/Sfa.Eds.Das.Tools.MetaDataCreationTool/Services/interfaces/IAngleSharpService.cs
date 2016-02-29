namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IAngleSharpService
    {
        IList<string> GetLinks(string fromUrl, string selector, string textInTitle);
    }
}