using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Inventory.Models;

namespace Inventory.Pages.CostCenters
{
    [CookieAuth("AllowedAccessCookieValue")]
    public class CreateModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public CreateModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CostCenter CostCenter { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.CostCenters == null || CostCenter == null)
            {
                return Page();
            }

            _context.CostCenters.Add(CostCenter);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
