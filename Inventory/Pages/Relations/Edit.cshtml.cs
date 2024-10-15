using Inventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Pages.Relations
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
        public Relation Relation { get; set; } = default!;
        [BindProperty]
        public Device Device { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Relations == null)
            {
                return NotFound();
            }

            var relation = await _context.Relations.FirstOrDefaultAsync(m => m.Id == id);
            if (relation == null)
            {
                return NotFound();
            }
            Relation = relation;
            var device = await _context.Devices.FirstOrDefaultAsync(m => m.Id == relation.DeviceId);
            Device = device;
            ViewData["ModelID"] = new SelectList(_context.Models, "Id", "Name");
            ViewData["AccountID"] = new SelectList(_context.Accounts, "Id", "Oa");
            ViewData["UserID"] = new SelectList(_context.Users, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("Entro nel post");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Device = await _context.Devices.FirstOrDefaultAsync(m => m.Id == Relation.DeviceId);
            _context.Attach(Relation).State = EntityState.Modified;
            //_context.Attach(Device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelationExists(Relation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Reattach Device and update state
            var existingDevice = await _context.Devices.FindAsync(Relation.DeviceId);
            if (existingDevice != null)
            {
                existingDevice.Sn = Device.Sn;
                existingDevice.ModelId = Device.ModelId;
                existingDevice.Zhe = Device.Zhe;
                existingDevice.Stato = Device.Stato;
                existingDevice.Data = Device.Data;
                existingDevice.Note = Device.Note;
                existingDevice.AccountId = Device.AccountId;

                _context.Attach(existingDevice).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(Relation.DeviceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }

        private bool RelationExists(int id)
        {
            return (_context.Relations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool DeviceExists(int? id)
        {
            return (_context.Devices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
