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
        public int CurrentPage { get; set; }

        public void OnGet(int currentPage = 1)
        {
            CurrentPage = currentPage;
        }
    }
}