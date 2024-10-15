using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;

namespace Inventory.Pages.Models
{
    public class IndexModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public IndexModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }

        public IList<Inventory.Models.Model> Model { get;set; } = default!;

        [BindProperty]
        public string? SearchName { get; set; }

        [BindProperty]
        public string? SearchDescription { get; set; }

        [BindProperty]
        public string? SearchBrandName { get; set; }

        [BindProperty]
        public string? SubmitSource { get; set; }

        [BindProperty]
        public string? ButtonSource { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Models != null)
            {
                Model = await _context.Models
                    .Include(m => m.Brand)
                    .OrderBy(m => m.Description)
                    .ToListAsync();
                ButtonSource = "buttonDescriptionA";
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
                    results = _context.Models
                        .Include(m => m.Brand) // Include la navigazione verso il Brand
                        .AsEnumerable()
                        .Where(i => i.Name != null && i.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, i.Name })
                        .OrderBy(i => i.Name);
                    break;
                case "Description":
                    results = _context.Models                       
                        .AsEnumerable()
                        .Where(i => i.Description != null && i.Description.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, i.Description })
                        .OrderBy(i => i.Description);
                    break;
                case "BrandName":
                    results = _context.Models
                        .Include(m => m.Brand) // Include la navigazione verso il Brand
                        .AsEnumerable()
                        .Where(i => i.Brand != null && i.Brand.Name != null && i.Brand.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, BrandName = i.Brand.Name })
                        .OrderBy(i => i.BrandName);
                    break;
                default:
                    results = new List<object>();
                    break;
            }

            return new JsonResult(results);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var query = _context.Models
                .Include(m => m.Brand)
                .AsQueryable();

            if (string.IsNullOrEmpty(SubmitSource))
            {
                Model = await query.ToListAsync();
                return Page();
            }

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(m => m.Name != null && EF.Functions.Like(m.Name, $"%{SearchName}%"));
            }

            if (!string.IsNullOrEmpty(SearchDescription))
            {
                query = query.Where(m => m.Description != null && EF.Functions.Like(m.Description, $"%{SearchDescription}%"));
            }

            if (!string.IsNullOrEmpty(SearchBrandName))
            {
                query = query.Where(m => m.Brand != null && EF.Functions.Like(m.Brand.Name, $"%{SearchBrandName}%"));
            }

            switch (ButtonSource)
            {
                case "buttonNameA":
                    query = query.OrderBy(m => m.Name);
                    break;
                case "buttonNameZ":
                    query = query.OrderByDescending(m => m.Name);
                    break;
                case "buttonDescriptionA":
                    query = query.OrderBy(m => m.Description);
                    break;
                case "buttonDescriptionZ":
                    query = query.OrderByDescending(m => m.Description);
                    break;
                case "buttonBrandNameA":
                    query = query.OrderBy(m => m.Brand.Name);
                    break;
                case "buttonBrandNameZ":
                    query = query.OrderByDescending(m => m.Brand.Name);
                    break;
                default:

                    break;
            }

            Model = await query.ToListAsync();

            return Page();
        }
    }
}
