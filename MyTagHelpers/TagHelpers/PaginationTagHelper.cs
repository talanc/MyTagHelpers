using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTagHelpers.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        private readonly IHtmlHelper _htmlHelper;

        public PaginationTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }
        
        public string PageAttribute { get; set; }
        
        public int PageCurrent { get; set; }
        
        public int PageFirst { get; set; }
        
        public int PageLast { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
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
                    content += $"<li><a href='?{PageAttribute}={i}'>{i}</a></li>";
                }
                content += $"<li style='pointer-events: none;'><span>...</span></li>";
            }
            content += "</ul>";

            output.Content.SetHtmlContent(content);
        }
    }
}
