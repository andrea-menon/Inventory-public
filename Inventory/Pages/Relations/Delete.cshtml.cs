using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Pages.Relations
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
        public Relation Relation { get; set; } = default!;
        [BindProperty]
        public Device Device { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Relations == null)
            {
                return NotFound();
            }

            var relation = await _context.Relations
                .Include(m => m.User)
                .ThenInclude(m => m.CostCenter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (relation == null)
            {
                return NotFound();
            }
            Relation = relation;
            var device = await _context.Devices
                .Include(m => m.Model)
                .ThenInclude(m => m.Brand)
                .Include(m => m.Account)
                .FirstOrDefaultAsync(m => m.Id == relation.DeviceId);
            Device = device;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Relations == null)
            {
                return NotFound();
            }
            var relation = await _context.Relations.FindAsync(id);
            var device = await _context.Devices.FindAsync(relation.DeviceId);

            if (relation != null)
            {
                Relation = relation;
                Device = device;
                try
                {
                    _context.Relations.Remove(Relation);
                    await _context.SaveChangesAsync();
                    _context.Devices.Remove(Device);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    TempData["Avviso"] = "Si è verificato un errore imprevisto";
                    return RedirectToPage("Delete", new { id = id });
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
