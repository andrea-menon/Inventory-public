using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventory.Models;

namespace Inventory.Pages.Users
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
            ViewData["CostCenterID"] = new SelectList(_context.CostCenters, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Users == null || User == null)
            {
                return Page();
            }

            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
