namespace Sfa.Eds.Das.Core.Extensions
{
    using System.Web.Mvc;

    public static class HtmlExtensions
    {
        public static MvcHtmlString RenderAIfExists(this HtmlHelper htmlHelper, string title, string source, string classes)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(title))
            {
                return new MvcHtmlString(string.Empty);
            }

            var html = $"<a href=\"{source}\" class=\"{classes}\">{title}</a>";

            return new MvcHtmlString(html);
        }
    }
}