using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventory.Models;
using Microsoft.Data.SqlClient;

namespace Inventory.Pages.Devices
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
            ViewData["ModelID"] = new SelectList(_context.Models, "Id", "Name");
            ViewData["AccountID"] = new SelectList(_context.Accounts, "Id", "Oa");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Device Device { get; set; } = default!;
        [BindProperty]
        public Relation Relation { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Devices == null || Device == null || _context.Relations == null || Relation == null)
            {
                return Page();
            }
            
            _context.Devices.Add(Device);
            await _context.SaveChangesAsync();
            Relation.DeviceId = Device.Id;
            _context.Relations.Add(Relation);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
