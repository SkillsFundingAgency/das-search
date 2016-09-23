using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Parser.Html;
using Sfa.Das.Sas.Indexer.ApplicationServices.Http;
using Sfa.Das.Sas.Tools.MetaDataCreationTool.Services.Interfaces;

namespace Sfa.Das.Sas.Tools.MetaDataCreationTool.Services
{
    using Sfa.Das.Sas.Indexer.Core.Logging;
    using Sfa.Das.Sas.Indexer.Core.Logging.Metrics;
    using Sfa.Das.Sas.Indexer.Core.Logging.Models;

    public class AngleSharpService : IAngleSharpService
    {
        private readonly IHttpGet _httpGet;

        private readonly ILog _logger;

        public AngleSharpService(IHttpGet httpGet, ILog logger)
        {
            _httpGet = httpGet;
            _logger = logger;
        }

        public IList<string> GetLinks(string fromUrl, string selector, string textInTitle)
        {
            if (string.IsNullOrEmpty(fromUrl))
            {
                return new List<string>();
            }

            try
            {
                var timing = ExecutionTimer.GetTiming(() => _httpGet.Get(fromUrl, null, null));

                var logEntry = new DependencyLogEntry
                {
                    Identifier = "AngleSharp",
                    ResponseTime = timing.ElaspedMilliseconds,
                    Url = fromUrl
                };
                _logger.Debug("AngleSharp", logEntry);

                var parser = new HtmlParser();
                var result = parser.Parse(timing.Result);
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