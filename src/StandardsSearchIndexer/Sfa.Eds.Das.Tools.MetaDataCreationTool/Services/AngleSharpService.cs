namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AngleSharp;

    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class AngleSharpService : IAngleSharpService
    {
        public IList<string> GetLinks(string fromUrl, string selector, string textInTitle)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var document = BrowsingContext.New(config).OpenAsync(fromUrl);

            var result = document.Result;
            var all = result.QuerySelectorAll(selector);

            return all.Where(x => x.InnerHtml.Contains(textInTitle)).Select(x => x.GetAttribute("href").ToString()).ToList();
        }
    }
}
