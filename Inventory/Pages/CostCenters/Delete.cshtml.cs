using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Pages.CostCenters
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
        public CostCenter CostCenter { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CostCenters == null)
            {
                return NotFound();
            }

            var costCenter = await _context.CostCenters.FirstOrDefaultAsync(m => m.Id == id);

            if (costCenter == null)
            {
                return NotFound();
            }
            else
            {
                CostCenter = costCenter;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CostCenters == null)
            {
                return NotFound();
            }
            var costCenter = await _context.CostCenters.FindAsync(id);

            if (costCenter != null)
            {
                CostCenter = costCenter;
                try
                {
                    _context.CostCenters.Remove(CostCenter);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    TempData["Avviso"] = "Non è possibile cancellare il centro di costo in quanto è associato ad almeno un utente";
                    return RedirectToPage("Delete", new { id = id });
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
