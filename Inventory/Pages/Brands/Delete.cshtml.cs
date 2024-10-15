using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;

namespace Inventory.Pages.Brands
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
      public Brand Brand { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);

            if (brand == null)
            {
                return NotFound();
            }
            else 
            {
                Brand = brand;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }
            var brand = await _context.Brands.FindAsync(id);

            if (brand != null)
            {
                Brand = brand;
                try
                {
                    _context.Brands.Remove(Brand);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    TempData["Avviso"] = "Non è possibile cancellare la marca in quanto è associata ad almeno un modello";
                    return RedirectToPage("Delete", new { id = id });
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
