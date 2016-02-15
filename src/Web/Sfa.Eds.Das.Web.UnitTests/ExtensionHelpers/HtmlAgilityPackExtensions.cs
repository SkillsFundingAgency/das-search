namespace Sfa.Eds.Das.Web.UnitTests.ExtensionHelpers
{
    using AngleSharp.Parser.Html;

    using HtmlAgilityPack;

    public static class HtmlAgilityPackExtensions
    {
        public static AngleSharp.Dom.Html.IHtmlDocument ToAngleSharp(this HtmlDocument document)
        {
            var html = document?.DocumentNode?.OuterHtml;
            var ss = new HtmlParser().Parse(html ?? string.Empty);

            return ss;
        }
    }
}