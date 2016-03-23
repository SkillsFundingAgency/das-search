namespace Sfa.Eds.Das.Tools.MetaDataCreationTool.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using AngleSharp.Parser.Html;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Http;
    using Sfa.Eds.Das.Tools.MetaDataCreationTool.Services.Interfaces;

    public class AngleSharpService : IAngleSharpService
    {
        private readonly IHttpGet _httpGet;

        public AngleSharpService(IHttpGet httpGet)
        {
            _httpGet = httpGet;
        }

        public IList<string> GetLinks(string fromUrl, string selector, string textInTitle)
        {
            var data = _httpGet.Get(fromUrl, null, null);
            var parser = new HtmlParser();
            var result = parser.Parse(data);
            var all = result.QuerySelectorAll(selector);

            return all.Where(x => x.InnerHtml.Contains(textInTitle)).Select(x => x.GetAttribute("href")).ToList();
        }
    }
}