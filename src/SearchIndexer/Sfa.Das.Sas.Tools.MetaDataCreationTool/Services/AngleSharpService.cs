using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Parser.Html;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    public class AngleSharpService : IAngleSharpService
    {
        private readonly IHttpGet _httpGet;

        public AngleSharpService(IHttpGet httpGet)
        {
            _httpGet = httpGet;
        }

        public IList<string> GetLinks(string fromUrl, string selector, string textInTitle)
        {
            if (string.IsNullOrEmpty(fromUrl))
            {
                return new List<string>();
            }

            try
            {
                var data = _httpGet.Get(fromUrl, null, null);
                var parser = new HtmlParser();
                var result = parser.Parse(data);
                var all = result.QuerySelectorAll(selector);

                return all.Where(x => x.InnerHtml.Contains(textInTitle)).Select(x => x.GetAttribute("href")).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}