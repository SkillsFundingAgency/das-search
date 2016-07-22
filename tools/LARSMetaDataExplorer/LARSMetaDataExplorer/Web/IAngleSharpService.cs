using System.Collections.Generic;

namespace LARSMetaDataExplorer.Web
{
    public interface IAngleSharpService
    {
        IList<string> GetLinks(string fromUrl, string selector, string textInTitle);
    }
}