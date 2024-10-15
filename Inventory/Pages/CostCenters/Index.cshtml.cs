using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Pages.CostCenters
{
    public class IndexModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public IndexModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }

        public IList<CostCenter> CostCenter { get; set; } = default!;

        [BindProperty]
        public string? SearchName {  get; set; }

        [BindProperty]
        public string? SearchDescription { get; set; }

        [BindProperty]
        public string? SubmitSource { get; set; }

        [BindProperty]
        public string? ButtonSource { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.CostCenters != null)
            {
                CostCenter = await _context.CostCenters
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
                    results = _context.CostCenters
                        .AsEnumerable()
                        .Where(i => i.Name != null && i.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Name = i.Name })
                        .OrderBy(i => i.Name);
                    break;
                case "Description":
                    results = _context.CostCenters
                        .AsEnumerable()
                        .Where(i => i.Description != null && i.Description.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Description = i.Description })
                        .OrderBy(i => i.Description);
                    break;
                default:
                    results = new List<object>();
                    break;
            }

            return new JsonResult(results);
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var query = _context.CostCenters
                .AsQueryable();

            if (string.IsNullOrEmpty(SubmitSource))
            {
                CostCenter = await query.ToListAsync();
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
                default:

                    break;
            }

            CostCenter = await query.ToListAsync();

            return Page();
        }
    }
}
