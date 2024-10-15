using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;

namespace Inventory.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public IndexModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }

        public IList<Brand> Brand { get;set; } = default!;

        [BindProperty]
        public string? SearchName { get; set; }

        [BindProperty]
        public string? SubmitSource { get; set; }

        [BindProperty]
        public string? ButtonSource { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Brands != null)
            {
                Brand = await _context.Brands
                    .OrderBy(m => m.Name)
                    .ToListAsync();
                ButtonSource = "buttonNameA";
            }
        }

        public JsonResult OnGetSearch(string term, string column)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(column))
            {
                return new JsonResult(new List<object>());
            }

            IEnumerable<object> results;

            switch (column)
            {
                case "Name":
                    results = _context.Brands
                        .AsEnumerable()
                        .Where(i => i.Name != null && i.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, i.Name })
                        .OrderBy(i => i.Name);
                    break;
                default:
                    results = new List<object>();
                    break;
            }

            return new JsonResult(results);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var query = _context.Brands
                    .AsQueryable();

            if (string.IsNullOrEmpty(SubmitSource))
            {
                Brand = await query.ToListAsync();
                return Page();
            }

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(m => EF.Functions.Like(m.Name, $"%{SearchName}%"));
            }

            switch (ButtonSource)
            {
                case "buttonNameA":
                    query = query.OrderBy(m => m.Name);
                    break;
                case "buttonNameZ":
                    query = query.OrderByDescending(m => m.Name);
                    break;
                default:

                    break;
            }

            Brand = await query.ToListAsync();

            return Page();
        }
    }
}
