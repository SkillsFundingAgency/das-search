namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AngleSharp;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.interfaces;

    public class AngleSharpService : IAngleSharpService
    {
        public IList<string> GetLinks(string fromUrl, string selector, string textInTitle)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var document = BrowsingContext.New(config).OpenAsync(fromUrl);

            var all = document.Result.QuerySelectorAll(selector);

            return all.Where(x => x.InnerHtml.Contains(textInTitle)).Select(x => x.GetAttribute("href").ToString()).ToList();
        }
    }
}
