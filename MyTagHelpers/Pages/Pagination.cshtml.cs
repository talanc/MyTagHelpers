using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyTagHelpers.Pages
{
    public class PaginationModel : PageModel
    {
        public int? PageNumber { get; set; }

        public string Value1 { get; set; }

        public string Value2 { get; set; }

        public void OnGet(int? pageNumber = null, string value1 = null, string value2 = null)
        {
            PageNumber = pageNumber;
            Value1 = value1;
            Value2 = value2;
        }
    }
}