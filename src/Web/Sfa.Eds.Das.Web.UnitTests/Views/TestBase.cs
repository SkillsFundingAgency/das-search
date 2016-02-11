namespace Sfa.Eds.Das.Web.UnitTests.Views
{
    using System.Linq;

    using AngleSharp.Dom.Html;

    public class TestBase
    {
        protected string GetPartial(IHtmlDocument html, string selector)
        {
            return html?.QuerySelectorAll(selector)?.FirstOrDefault()?.TextContent?.Replace("\r", string.Empty).Trim() ?? string.Empty;
        }
    }
}