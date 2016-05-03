using System.Web;
using System.Web.Mvc;

namespace Sfa.Das.Sas.Web.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString RenderAIfExists(this HtmlHelper htmlHelper, string title, string source, string classes)
        {
            return RenderAIfExists(htmlHelper, title, source, classes, "_self");
        }

        public static MvcHtmlString RenderAIfExists(this HtmlHelper htmlHelper, string title, string source, string classes, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(title))
            {
                return new MvcHtmlString(string.Empty);
            }

            var html = $"<a href=\"{source}\" target=\"{target}\" class=\"{classes}\">{title}</a>";

            return new MvcHtmlString(html);
        }

        public static HtmlString MarkdownToHtml(this HtmlHelper htmlHelper, string markdownText)
        {
            if (!string.IsNullOrEmpty(markdownText))
            {
                return new HtmlString("<div class=\"markdown\">" + CommonMark.CommonMarkConverter.Convert(markdownText.Replace("\\r", "\r").Replace("\\n", "\n")) + "</div>");
            }

            return new HtmlString(string.Empty);
        }
    }
}