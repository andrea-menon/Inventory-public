using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;

namespace Inventory.Pages.Models
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
        public Model Model { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .Include(m => m.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }
            else 
            {
                Model = model;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }
            var model = await _context.Models.FindAsync(id);

            if (model != null)
            {
                Model = model;
                try
                {
                    _context.Models.Remove(Model);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    TempData["Avviso"] = "Non è possibile cancellare il modello in quanto è associato ad almeno un dispositivo";
                    return RedirectToPage("Delete", new { id = id });
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
