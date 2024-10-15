using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;


namespace Inventory.Pages.Devices
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
        public Device Device { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Devices == null)
            {
                return NotFound();
            }

            var device = await _context.Devices
                .Include(m => m.Model)
                .ThenInclude(m => m.Brand)
                .Include(m => m.Account)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (device == null)
            {
                return NotFound();
            }
            else
            {
                Device = device;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Devices == null)
            {
                return NotFound();
            }
            var device = await _context.Devices.FindAsync(id);

            if (device != null)
            {
                Device = device;
                try
                {
                    _context.Devices.Remove(Device);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    TempData["Avviso"] = "Non è possibile cancellare il dispositivo in quanto è associato ad almeno un utente";
                    return RedirectToPage("Delete", new { id = id });
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
