using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;

namespace Inventory.Pages.CostCenters
{
    [CookieAuth("AllowedAccessCookieValue")]
    public class EditModel : PageModel
    {
        private readonly Inventory.Data.InventoryContext _context;

        public EditModel(Inventory.Data.InventoryContext context)
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

            var costCenter = await _context.CostCenters.FirstOrDefaultAsync(a => a.Id == id);
            if (costCenter == null)
            {
                return NotFound();
            }
            CostCenter = costCenter;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CostCenter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostCenterExists(CostCenter.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CostCenterExists(int id)
        {
            return (_context.CostCenters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
