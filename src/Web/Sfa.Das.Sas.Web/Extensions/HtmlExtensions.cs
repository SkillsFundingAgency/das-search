using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Sfa.Das.Sas.Web.Extensions
{
    public static class HtmlExtensions
    {
        public static HtmlString RenderAIfExists(this HtmlHelper htmlHelper, string title, string source, string classes)
        {
            return RenderAIfExists(htmlHelper, title, source, classes, "_self");
        }

        public static HtmlString RenderAIfExists(this HtmlHelper htmlHelper, string title, string source, string classes, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(title))
            {
                return new HtmlString(string.Empty);
            }

            var html = $"<a href=\"{source}\" target=\"{target}\" class=\"{classes}\">{title}</a>";

            return new HtmlString(html);
        }

        public static HtmlString MarkdownToHtml(this HtmlHelper htmlHelper, string markdownText)
        {
            if (!string.IsNullOrEmpty(markdownText))
            {
                //TODO: LWA Replace CommonMark
                //return new HtmlString("<div class=\"markdown\">" + htmlHelper.Raw(CommonMark.CommonMarkConverter.Convert(markdownText.Replace("\\r", "\r").Replace("\\n", "\n"))) + "</div>");
                return new HtmlString("<div class=\"markdown\">" + "</div>");
            }

            return new HtmlString(string.Empty);
        }
    }
}