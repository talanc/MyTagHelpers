using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTagHelpers.TagHelpers
{
    [HtmlTargetElement("pagination", Attributes = AttributeAttributeName)]
    [HtmlTargetElement("pagination", Attributes = CurrentAttributeName)]
    [HtmlTargetElement("pagination", Attributes = FirstAttributeName)]
    [HtmlTargetElement("pagination", Attributes = LastAttributeName)]
    [HtmlTargetElement("pagination", Attributes = RouteValuesPrefix + "*")]
    [HtmlTargetElement("pagination", Attributes = RouteValuesDictionaryName)]
    public class PaginationTagHelper : TagHelper
    {
        private const string AttributeAttributeName = "page-attribute";
        private const string CurrentAttributeName = "page-current";
        private const string FirstAttributeName = "page-first";
        private const string LastAttributeName = "page-last";
        private const string RouteValuesDictionaryName = "page-all-route-data";
        private const string RouteValuesPrefix = "page-route-";

        private readonly IHtmlHelper _htmlHelper;

        public PaginationTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
            PageRouteValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        [HtmlAttributeName(AttributeAttributeName)]
        public string PageAttribute { get; set; }

        [HtmlAttributeName(CurrentAttributeName)]
        public int PageCurrent { get; set; }

        [HtmlAttributeName(FirstAttributeName)]
        public int PageFirst { get; set; }

        [HtmlAttributeName(LastAttributeName)]
        public int PageLast { get; set; }

        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> PageRouteValues { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var routeValues = "";
            if (PageRouteValues != null)
            {
                routeValues = "&" + string.Join("&", PageRouteValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }

            output.TagName = "nav";

            output.Attributes.SetAttribute("aria-label", "Page navigation");

            var content = "<ul class='pagination'>";
            for (var i = PageFirst; i <= PageLast; i++)
            {
                if (i == PageCurrent)
                {
                    content += $"<li class='active'><span>{i} <span class='sr-only'>(current)</span></span></li>";
                }
                else
                {
                    content += $"<li><a href='?{PageAttribute}={i}{routeValues}'>{i}</a></li>";
                }
                //content += $"<li style='pointer-events: none;'><span>...</span></li>";
            }
            content += "</ul>";

            output.Content.SetHtmlContent(content);
        }
    }
}
