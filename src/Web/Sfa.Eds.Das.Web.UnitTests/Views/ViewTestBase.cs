using AngleSharp.Dom;

namespace Sfa.Eds.Das.Web.UnitTests.Views
{
    using System.Linq;

    using AngleSharp.Dom;
    using AngleSharp.Dom.Html;

    public abstract class ViewTestBase
    {
        protected string GetPartial(IHtmlDocument html, string selector, int index = 1)
        {
            return GetTextContent(html?.QuerySelectorAll(selector), index)
                   ?? string.Empty;
        }

        protected string GetPartial(IElement html, string selector, int index = 1)
        {
            return GetTextContent(html?.QuerySelectorAll(selector), index)
                   ?? string.Empty;
        }

        protected IElement GetHtmlElement(IHtmlDocument html, string selector, int index = 1)
        {
            return html?.QuerySelectorAll(selector)[index - 1];
        }

        protected string GetPartialWhere(IHtmlDocument html, string selector, string textContent)
        {
            return
                html?.QuerySelectorAll(selector)?
                    .First(m => m.TextContent.Contains(textContent))?
                    .TextContent?.Replace("\r", string.Empty)
                    .Trim() ?? string.Empty;
        }

        private string GetTextContent(IHtmlCollection<IElement> querySelectorAll, int index)
        {
            return querySelectorAll?[index - 1]?.TextContent?.Replace("\r", string.Empty).Trim();
        }
    }
}