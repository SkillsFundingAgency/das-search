namespace Sfa.Eds.Das.Web.UnitTests.Views
{
    using System.Linq;

    using AngleSharp.Dom.Html;

    public abstract class ViewTestBase
    {
        protected string GetPartial(IHtmlDocument html, string selector)
        {
            return html?.QuerySelectorAll(selector)?.FirstOrDefault()?.TextContent?.Replace("\r", string.Empty).Trim()
                   ?? string.Empty;
        }

        protected string GetPartialWhere(IHtmlDocument html, string selector, string textContent)
        {
            return
                html?.QuerySelectorAll(selector)?
                    .First(m => m.TextContent.Contains(textContent))?
                    .TextContent?.Replace("\r", string.Empty)
                    .Trim() ?? string.Empty;
        }
    }
}