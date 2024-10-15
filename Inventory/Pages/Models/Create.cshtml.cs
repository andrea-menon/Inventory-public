using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventory.Data;
using Inventory.Models;

namespace Inventory.Pages.Models
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
            ViewData["BrandID"] = new SelectList(_context.Brands, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Model Model { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        // public async Task<IActionResult> Create([Bind("Id,CustomerID,StoreID,RoomID,ScheduleID,CreatoIl,Away")] Order order)
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Models == null || Model == null)
            {
                return Page();
            }

            _context.Models.Add(Model);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
