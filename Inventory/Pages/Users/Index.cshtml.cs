using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Inventory.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public IndexModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }

        public IList<Inventory.Models.User> User { get; set; } = default!;

        [BindProperty]
        public string? SearchName { get; set; }

        [BindProperty]
        public string? SearchDiAccount { get; set; }

        [BindProperty]
        public string? SearchCostCenterDescription { get; set; }

        [BindProperty]
        public string? SearchCostCenterName { get; set; }

        [BindProperty]
        public string? SubmitSource { get; set; }

        [BindProperty]
        public string? ButtonSource { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Users != null)
            {
                User = await _context.Users
                .Include(m => m.CostCenter)
                .OrderBy(m =>  m.Name)
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
                    results = _context.Users
                        .AsEnumerable()
                        .Where(i => i.Name != null && i.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, i.Name })
                        .OrderBy(i => i.Name);
                    break;
                case "DiAccount":
                    results = _context.Users
                        .AsEnumerable()
                        .Where(i => i.Di != null && i.Di.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, i.Di })
                        .OrderBy(i => i.Di);
                    break;
                case "CostCenterDescription":
                    results = _context.Users
                        .Include(m => m.CostCenter)
                        .AsEnumerable()
                        .Where(i => i.CostCenter != null && i.CostCenter.Description != null && i.CostCenter.Description.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, CostCenterDescription = i.CostCenter.Description })
                        .OrderBy(i => i.CostCenterDescription);
                    break;
                case "CostCenterName":
                    results = _context.Users
                        .Include(m => m.CostCenter)
                        .AsEnumerable()
                        .Where(i => i.CostCenter != null && i.CostCenter.Name != null && i.CostCenter.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, CostCenterName = i.CostCenter.Name })
                        .OrderBy(i => i.CostCenterName);
                    break;
                default:
                    results = new List<object>();
                    break;
            }

            return new JsonResult(results);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var query = _context.Users
                    .Include(m => m.CostCenter)
                    .AsQueryable();

            if (string.IsNullOrEmpty(SubmitSource))
            {
                User = await query.ToListAsync();
                return Page();
            }

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(m => EF.Functions.Like(m.Name, $"%{SearchName}%"));
            }

            if (!string.IsNullOrEmpty(SearchDiAccount))
            {
                query = query.Where(m => m.Di != null && EF.Functions.Like(m.Di, $"%{SearchDiAccount}%"));
            }

            if (!string.IsNullOrEmpty(SearchCostCenterDescription))
            {
                query = query.Where(m => m.CostCenter != null && m.CostCenter.Description != null && EF.Functions.Like(m.CostCenter.Description, $"%{SearchCostCenterDescription}%"));
            }

            if (!string.IsNullOrEmpty(SearchCostCenterName))
            {
                query = query.Where(m => m.CostCenter != null && m.CostCenter.Name != null && EF.Functions.Like(m.CostCenter.Name, $"%{SearchCostCenterName}%"));
            }

            switch (ButtonSource)
            {
                case "buttonNameA":
                    query = query.OrderBy(m => m.Name);
                    break;
                case "buttonNameZ":
                    query = query.OrderByDescending(m => m.Name);
                    break;
                case "buttonDiAccountA":
                    query = query.OrderBy(m => m.Di);
                    break;
                case "buttonDiAccountZ":
                    query = query.OrderByDescending(m => m.Di);
                    break;
                case "buttonCostCenterDescriptionA":
                    query = query.OrderBy(m => m.CostCenter.Description);
                    break;
                case "buttonCostCenterDescriptionZ":
                    query = query.OrderByDescending(m => m.CostCenter.Description);
                    break;
                case "buttonCostCenterNameA":
                    query = query.OrderBy(m => m.CostCenter.Name);
                    break;
                case "buttonCostCenterNameZ":
                    query = query.OrderByDescending(m => m.CostCenter.Name);
                    break;
                default:

                    break;
            }

            User = await query.ToListAsync();

            return Page();
        }
    }
}
