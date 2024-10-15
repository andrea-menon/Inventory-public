using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Pages.Users
{
    [CookieAuth("AllowedAccessCookieValue")]
    public class DeleteModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public DeleteModel(Inventory.Data.InventoryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(m => m.CostCenter)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                User = user;
                try
                {
                    _context.Users.Remove(User);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    TempData["Avviso"] = "Non è possibile cancellare l'utente in quanto è associato ad almeno un dispositivo";
                    return RedirectToPage("Delete", new { id = id });
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
