using System.Collections.Generic;

namespace LARSMetaDataToolBox.Web
{
    public interface IAngleSharpService
    {
        IList<string> GetLinks(string fromUrl, string selector, string textInTitle);
    }
}