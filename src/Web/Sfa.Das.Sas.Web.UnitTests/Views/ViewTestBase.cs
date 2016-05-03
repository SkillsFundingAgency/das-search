using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;

namespace Sfa.Das.Sas.Web.UnitTests.Views
{
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

        protected string GetAttribute(IHtmlDocument html, string selector, string attribute, int index = 1)
        {
            return html?.QuerySelectorAll(selector)[index - 1]?.GetAttribute(attribute);
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