using AngleSharp.Parser.Html;
using HtmlAgilityPack;

namespace Sfa.Das.Sas.MetadataTool.UnitTests.Extensions
{
    public static class HtmlAgilityPackExtensions
    {
        public static AngleSharp.Dom.Html.IHtmlDocument ToAngleSharp(this HtmlDocument document)
        {
            var html = document?.DocumentNode?.OuterHtml;
            return new HtmlParser().Parse(html ?? string.Empty);
        }
    }
}
