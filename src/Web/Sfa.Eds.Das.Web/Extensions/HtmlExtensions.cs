namespace Sfa.Eds.Das.Web.Extensions
{
    using System.Web;
    using System.Web.Mvc;

    public static class HtmlExtensions
    {
        public static MvcHtmlString RenderAIfExists(this HtmlHelper htmlHelper, string title, string source, string classes, string target = "_self")
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(title))
            {
                return new MvcHtmlString(string.Empty);
            }

            if (!source.Contains("http://"))
            {
                source = $"http://{source}";
            }

            var html = $"<a href=\"{source}\" target=\"{target}\" class=\"{classes}\">{title}</a>";

            return new MvcHtmlString(html);
        }

        public static HtmlString MarkdownToHtml(this HtmlHelper htmlHelper, string markdownText)
        {
            if (!string.IsNullOrEmpty(markdownText))
            {
                return new HtmlString(CommonMark.CommonMarkConverter.Convert(markdownText.Replace("\\r", "\r").Replace("\\n", "\n")));
            }

            return new HtmlString(string.Empty);
        }
    }
}