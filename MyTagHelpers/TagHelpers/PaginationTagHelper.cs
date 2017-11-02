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
    [HtmlTargetElement("pagination", Attributes = NumAttributeName)]
    [HtmlTargetElement("pagination", Attributes = LastAttributeName)]
    [HtmlTargetElement("pagination", Attributes = RouteValuesPrefix + "*")]
    [HtmlTargetElement("pagination", Attributes = RouteValuesDictionaryName)]
    public class PaginationTagHelper : TagHelper
    {
        private const string AttributeAttributeName = "page-attribute";
        private const string NumAttributeName = "page-num";
        private const string LastAttributeName = "page-last";
        private const string RouteValuesDictionaryName = "page-all-route-data";
        private const string RouteValuesPrefix = "page-route-";

        private readonly IHtmlHelper _htmlHelper;

        public PaginationTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        [HtmlAttributeName(AttributeAttributeName)]
        public string PageAttribute { get; set; }

        [HtmlAttributeName(NumAttributeName)]
        public int PageNum { get; set; }

        [HtmlAttributeName(LastAttributeName)]
        public int PageLast { get; set; }

        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> PageRouteValues { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (PageNum <= 0)
            {
                PageNum = 0;
            }

            if (PageLast < 1)
            {
                PageLast = 1;
            }

            if (PageNum > PageLast)
            {
                PageNum = PageLast;
            }

            var routeValues = "";
            if (PageRouteValues?.Count() > 0)
            {
                routeValues = "&" + string.Join("&", PageRouteValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }

            output.TagName = "nav";

            output.Attributes.SetAttribute("aria-label", "Page navigation");

            var content = "<ul class='pagination'>";

            Action<int> addPage = (pg) =>
            {
                if (pg == PageNum)
                {
                    content += $"<li class='active'><span>{pg} <span class='sr-only'>(current)</span></span></li>";
                }
                else if (pg != -1)
                {
                    content += $"<li><a href='?{PageAttribute}={pg}{routeValues}'>{pg}</a></li>";
                }
                else
                {
                    content += $"<li style='pointer-events: none;'><span>...</span></li>";
                }
            };
            
            var pages = new SortedSet<int>();
            pages.Add(1);
            if (PageLast >= 2) pages.Add(2);
            if (PageLast >= 3) pages.Add(3);
            if (PageNum - 2 > 1) pages.Add(PageNum - 2);
            if (PageNum - 1 > 1) pages.Add(PageNum - 1);
            pages.Add(PageNum);
            if (PageNum + 1 < PageLast) pages.Add(PageNum + 1);
            if (PageNum + 2 < PageLast) pages.Add(PageNum + 2);
            if (PageLast - 2 > PageNum) pages.Add(PageLast - 2);
            if (PageLast - 1 > PageNum) pages.Add(PageLast - 1);
            pages.Add(PageLast);

            var prevPage = pages.First();
            addPage(prevPage);
            foreach (var page in pages.Skip(1))
            {
                switch (page - prevPage)
                {
                    case 0:
                        break;
                    case 1:
                        addPage(page);
                        break;
                    case 2:
                        addPage(page - 1);
                        addPage(page);
                        break;
                    default:
                        addPage(-1);
                        addPage(page);
                        break;
                }

                prevPage = page;
            }
            
            content += "</ul>";

            output.Content.SetHtmlContent(content);
        }
    }
}
