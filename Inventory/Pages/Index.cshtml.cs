﻿using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Pages
{
    public class IndexModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;

         public IndexModel(ILogger<IndexModel> logger)
         {
             _logger = logger;
         }


        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            
            return Page();
        }
    }
}