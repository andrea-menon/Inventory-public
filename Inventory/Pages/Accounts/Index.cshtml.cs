using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;

namespace Inventory.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;
        public IndexModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }
        public IList<Inventory.Models.Account> Account { get; set; } = default!;

        [BindProperty]
        public string? SearchOa { get; set; }

        [BindProperty]
        public string? SearchRda { get; set; }

        [BindProperty]
        public string? SearchCar { get; set; }

        [BindProperty]
        public string? SubmitSource { get; set; }

        [BindProperty]
        public string? ButtonSource { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Accounts != null)
            {
                Account = await _context.Accounts
                    .OrderBy(m => m.Oa)
                    .ToListAsync();
                ButtonSource = "buttonOaA";
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
                case "Oa":
                    results = _context.Accounts
                        .AsEnumerable()
                        .Where(i => i.Oa != null && i.Oa.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Oa = i.Oa })
                        .OrderBy(i => i.Oa);
                    break;
                case "Rda":
                    results = _context.Accounts
                        .AsEnumerable()
                        .Where(i => i.Rda != null && i.Rda.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Rda = i.Rda })
                        .OrderBy(i => i.Rda);
                    break;
                case "Car":
                    results = _context.Accounts
                        .AsEnumerable()
                        .Where(i => i.Car != null && i.Car.Contains(term, StringComparison.OrdinalIgnoreCase))
                        .Select(i => new { i.Id, Car = i.Car })
                        .OrderBy(i => i.Car);
                    break;
                default:
                    results = new List<object>();
                    break;
            }

            return new JsonResult(results);
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var query = _context.Accounts
                .AsQueryable();

            if (string.IsNullOrEmpty(SubmitSource))
            {
                Account = await query.ToListAsync();
                return Page();
            }

            if (!string.IsNullOrEmpty(SearchOa))
            {
                query = query.Where(m => m.Oa != null && EF.Functions.Like(m.Oa, $"%{SearchOa}%"));
            }

            if (!string.IsNullOrEmpty(SearchRda))
            {
                query = query.Where(m => m.Rda != null && EF.Functions.Like(m.Rda, $"%{SearchRda}%"));
            }

            if (!string.IsNullOrEmpty(SearchCar))
            {
                query = query.Where(m => m.Car != null && EF.Functions.Like(m.Car, $"%{SearchCar}%"));
            }

            switch (ButtonSource)
            {
                case "buttonOaA":
                    query = query.OrderBy(m => m.Oa);
                    break;
                case "buttonOaZ":
                    query = query.OrderByDescending(m => m.Oa);
                    break;
                case "buttonRdaA":
                    query = query.OrderBy(m => m.Rda);
                    break;
                case "buttonRdaZ":
                    query = query.OrderByDescending(m => m.Rda);
                    break;
                case "buttonCarA":
                    query = query.OrderBy(m => m.Car);
                    break;
                case "buttonCarZ":
                    query = query.OrderByDescending(m => m.Car);
                    break;
                default:

                    break;
            }

            Account = await query.ToListAsync();

            return Page();
        }
    }
}
