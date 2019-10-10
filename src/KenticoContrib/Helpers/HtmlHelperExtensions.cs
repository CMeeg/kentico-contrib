using System.Web.Mvc;

namespace KenticoContrib.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Metadata(this HtmlHelper htmlHelper, string property, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return MvcHtmlString.Empty;
            }

            return MvcHtmlString.Create($"<meta property=\"{property}\" content=\"{content}\">");
        }
    }
}